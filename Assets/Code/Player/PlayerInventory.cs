using Newtonsoft.Json;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InventorySend
{
    public string playerName;
    public List<Item> items = new List<Item>();
    public float hp, water, hungry;
}


public class PlayerInventory : MonoBehaviourPun
{
    public List<Item> items = new List<Item>(5);
    [SerializeField] int selected = 1;
    [SerializeField] int oldselected = 1;
    public Transform hand;
    [SerializeField] Transform skinHand;
    public bool cooldown;
    [SerializeField]
    Animator animator;
    bool change;
    [SerializeField]
    HandItem hands;
    float sendTime;
    [SerializeField]
    List<Craft.CraftType> craftTypes = new List<Craft.CraftType>() { Craft.CraftType.Player};
    
    public bool CheckCraftType(Craft.CraftType craftType)
    {
        return craftTypes.Contains(craftType);
    }

    public List<Craft.CraftType> GetCraftTypes()
    {
        return craftTypes;
    }
    public bool AddCraftType(Craft.CraftType craftType)
    {
        if (!craftTypes.Contains(craftType))
        {
            craftTypes.Add(craftType);
            return true;
        }
        return false;
    }
    public void RemoveCraftType(Craft.CraftType craftType)
    {
        craftTypes.Remove(craftType);
    }

    
    
    private void Start()
    {
        if (photonView.IsMine)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null) items[i] = items[i].Clone();
            }
            Set();
        }
    }

    public bool IsSelected(int id)
    {
        return selected == id;
    }
    [PunRPC]
    public void SpawnItemInHand(string name_of_item)
    {
        foreach (Transform item in photonView.IsMine ? hand : skinHand)
        {
            Destroy(item.gameObject);
        }

        var it = StaticManager.ItemByName(name_of_item);
        var obj = Instantiate(it.prefab, photonView.IsMine ? hand : skinHand);

        foreach (var item in obj.GetComponentsInChildren<Collider>())
        {
            Destroy(item);
        }
        if (!photonView.IsMine)
        {
            foreach (var item in obj.GetComponentsInChildren<MonoBehaviour>())
            {
                if (!(item is ItemPlace) && !(item is PlaceItemExecuter))
                    Destroy(item);
            }
            foreach (var item in obj.GetComponentsInChildren<Renderer>())
            {
                item.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            sendTime += Time.deltaTime;
            
            if (!cooldown)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (Input.GetKey((i + 1).ToString()))
                    {
                        selected = i;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (GetItem() != null)
                {
                    items[selected].value -= 1;

                    Drop(GetItem());

                    if (GetItem().value == 0)
                    {
                        items[selected] = null; change = true;
                    }
                }
                InventoryRenderItems.inventoryRender.Set();
            }

            if (selected != oldselected || change)
            {
                cooldown = true;
                animator.Play("ReSelect");
                change = false;
            }


            if (sendTime > 5)
            {
                SendData();
                sendTime = 0;
            }
        }
    }

    public void Drop(Item item)
    {
        float localForwardVelocity = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
        ChangesManager.cm.gameObject.GetPhotonView().RPC(
            "CreateDropItem",
            RpcTarget.All,
            "Drop",
            hand.transform.position,
            transform.rotation,
            item.name, 1, Camera.main.transform.forward * ((localForwardVelocity / 2) + 1));
    }

    public void DropAllInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                Drop(items[i]);
            }
        }
    }

    public void SendData()
    {
        var s = GetComponent<PlayerStats>();
        var h = GetComponent<LiveObject>();
        var data = new InventorySend() { hp = h.health, hungry = s.hunger, water = s.water, items = items, playerName = gameObject.GetPhotonView().Owner.NickName };
        FindObjectOfType<ChangesManager>().gameObject.GetPhotonView().RPC("SendPlayerData", RpcTarget.MasterClient, JsonConvert.SerializeObject(data));
    }

    public bool RemoveItem(string itemName, int value)
    {
        var item = items.Find(x => x != null && x.itemName == itemName && x.value >= value);
        if (item != null)
        {
            int id = items.FindIndex(x => x != null && x.itemName == itemName && x.value >= value);
            item.value -= value;
            if (item is PlaceItem)
            {
                change = true;
            }
            if (item.value <= 0)
            {
                Destroy(item);
                items[id] = null;
                if (IsSelected(id))
                {
                    Set();
                }
                change = true;
                return true;
            }
        }
        return false;
    }
    public void Set()
    {
        
        foreach (Transform item in hand)
        {
            Destroy(item.gameObject);
        }
        GetComponent<ItemsData>().handsAnim.runtimeAnimatorController = null;
        if (items[selected] != null)
        {
            if (items[selected].prefab != null)
            {
                photonView.RPC("SpawnItemInHand", RpcTarget.AllBuffered, items[selected].name);
            }
            if (items[selected] is HandItem)
            {
                GetComponent<ItemsData>().handsAnim.runtimeAnimatorController = (items[selected] as HandItem).animatorController;
            }
        }
        else
        {
            photonView.RPC("SpawnItemInHand", RpcTarget.AllBuffered, hands.name);
            GetComponent<ItemsData>().handsAnim.runtimeAnimatorController = hands.animatorController;
        }
        oldselected = selected;
        cooldown = false;
    }

    public Item GetItem()
    {
        return items[selected];
    }
    public Item GetItem(bool hand)
    {
        if (items[selected] != null)
        {
            return items[selected];
        }
        else
        {
            return hands;
        }
    }
    public bool AddItem(Drop drop)
    {
        bool added = false;
        var finded = items.FindAll(x => x != null && x.name == drop.item.name && x.value < x.maxValue);
        if (finded.Count != 0)
        {
            for (int i = 0; i < finded.Count; i++)
            {
                if (drop.item.value > 0 && finded[i].value + 1 <= finded[i].maxValue)
                {
                    finded[i].value++;
                    drop.item.value--;
                }
            }
        }
        if (drop.item.value != 0)
        {
            if (items.FindAll(x => x == null).Count != 0)
            {
                var n = items.FindIndex(x => x == null);
                items[n] = drop.item;
                if (n == selected)
                {
                    change = true;
                }
                drop.gameObject.GetPhotonView().RPC("DestroyRPC", RpcTarget.All);
                added = true;
            }
            else
            {
                drop.gameObject.GetPhotonView().RPC("SetValue", RpcTarget.All, drop.item.value);
            }
        }
        else
        {
            drop.gameObject.GetPhotonView().RPC("DestroyRPC", RpcTarget.All);
            added = true;
        }
        GetComponentInChildren<InventoryRenderItems>().ResetDisplay();
        return added;
    }


    public bool AddItem(Item item)
    {
        bool added = false;
        var finded = items.FindAll(x => x != null && x.name == item.name && x.value < x.maxValue);
        if (finded.Count != 0)
        {
            for (int i = 0; i < finded.Count; i++)
            {
                if (item.value > 0 && finded[i].value + 1 <= finded[i].maxValue)
                {
                    finded[i].value++;
                    item.value--;
                }
            }
        }
        if (item.value != 0)
        {
            if (items.FindAll(x => x == null).Count != 0)
            {
                var n = items.FindIndex(x => x == null);
                items[n] = item;
                if (n == selected)
                {
                    change = true;
                }
                added = true;
            }
        }
        else
        {
            added = true;
        }
        GetComponentInChildren<InventoryRenderItems>().ResetDisplay();
        return added;
    }
}
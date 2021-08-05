using Newtonsoft.Json;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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
    public int selected = 1;
    public int oldselected = 1;
    public Transform hand, skinHand;
    public bool cooldown;
    public Animator animator;
    bool change;
    public HandItem hands;
    float sendTime;
    public List<Craft.CraftType> craftTypes;
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
    [PunRPC]
    public void SpawnItemInHand(string itemName)
    {
        foreach (Transform item in photonView.IsMine ? hand : skinHand)
        {
            Destroy(item.gameObject);
        }
        var it = Resources.Load<Item>("ItemsObjects/" + itemName);
        var obj = Instantiate(it.prefab, photonView.IsMine ? hand : skinHand);
        if (!photonView.IsMine)
        {
            foreach (var item in obj.GetComponentsInChildren<MonoBehaviour>())
            {
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
            if (sendTime > 5)
            {
                SendData();
                sendTime = 0;
            }
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
                float localForwardVelocity = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.forward);
                if (GetItem() != null)
                {
                    items[selected].value -= 1;
                    ChangesManager.cm.gameObject.GetPhotonView().RPC(
                        "CreateDropItem",
                        RpcTarget.All,
                        "Drop",
                        hand.transform.position,
                        transform.rotation,
                        GetItem().name, 1, Camera.main.transform.forward * ((localForwardVelocity / 2) + 1));

                    if (GetItem().value == 0)
                    {
                        items[selected] = null; change = true;
                    }
                }
            }

            if (selected != oldselected || change)
            {
                cooldown = true;
                animator.Play("ReSelect");
                change = false;
            }
        }
    }

    public void SendData()
    {
        var s = GetComponent<PlayerStats>();
        var data = new InventorySend() { hp = s.health, hungry = s.hunger, water = s.water, items = items, playerName = gameObject.GetPhotonView().Owner.NickName };
        FindObjectOfType<ChangesManager>().gameObject.GetPhotonView().RPC("SendPlayerData", RpcTarget.MasterClient, JsonConvert.SerializeObject(data));
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
        //print("stop cooldown");
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
    public void AddItem(Drop drop)
    {
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
                //PhotonNetwork.Destroy(drop.gameObject);
            }
            else
            {
                drop.gameObject.GetPhotonView().RPC("SetValue", RpcTarget.All, drop.item.value);
            }
        }
        else
        {
            drop.gameObject.GetPhotonView().RPC("DestroyRPC", RpcTarget.All);
        }

        GetComponentInChildren<InventoryRenderItems>().ResetDisplay();
    }
}
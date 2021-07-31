using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(5);
    public int selected = 1;
    public int oldselected = 1;
    public Transform hand;
    public bool cooldown;
    public Animator animator;
    bool change;
    public HandItem hands;
    private void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null) items[i] =  items[i].Clone();
        }
        Set();
    }
    private void Update()
    {
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
                    GetItem().name, 1, Camera.main.transform.forward * ((localForwardVelocity/2)+1));

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
                Instantiate(items[selected].prefab, hand);
            }
            if (items[selected] is HandItem)
            {
                GetComponent<ItemsData>().handsAnim.runtimeAnimatorController = (items[selected] as HandItem).animatorController;
            }
        }
        else
        {
            Instantiate(hands.prefab, hand);
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
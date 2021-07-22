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
    private void Start()
    {
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

            if (selected != oldselected)
            {
                cooldown = true;
                animator.Play("ReSelect");
            }
        }
    }
    public void Set()
    {
        
        foreach (Transform item in hand)
        {
            Destroy(item.gameObject);
        }
        if (items[selected] != null)
        {
            if (items[selected].prefab != null)
            {
                Instantiate(items[selected].prefab, hand);
            }
            if (items[selected] is HandItem)
            {
                if ((items[selected] as HandItem).animatorController != null)
                {
                    GetComponent<ItemsData>().handsAnim.runtimeAnimatorController = (items[selected] as HandItem).animatorController;
                }
            }
        }
        //print("stop cooldown");
        oldselected = selected;
        cooldown = false;
    }

    public Item GetItem()
    {
        return items[selected];
    }

}

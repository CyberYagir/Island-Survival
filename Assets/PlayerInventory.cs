using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(5);
    public int selected = 1;
    public int oldselected = 1;
    public Transform hand;

    private void Update()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (Input.GetKey((i+1).ToString()))
            {
                selected = i;
            }
        }

        if (selected != oldselected)
        {
            foreach (Transform item in hand)
            {
                Destroy(item.gameObject);
            }
            if (items[selected] != null && items[selected].prefab != null)
            {
                Instantiate(items[selected].prefab, hand);
            }
            oldselected = selected;
        }
    }

    public Item GetItem()
    {
        return items[selected];
    }

}

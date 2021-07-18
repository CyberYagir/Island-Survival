using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(5);
    public int selected;

    private void Update()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (Input.GetKeyDown((i+1).ToString()))
            {
                selected = i;
            }
        }
    }

}

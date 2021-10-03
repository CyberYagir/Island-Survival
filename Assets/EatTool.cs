using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatTool : Usable
{
    public float eatTime;
    public void Eat()
    {
        if (playerInventory.GetItem() != null)
        {
            itemsData.handsAnim.SetLayerWeight(1, Mathf.Clamp01(itemsData.handsAnim.GetLayerWeight(1) + Time.deltaTime * 3));
            eatTime += Time.deltaTime;
            var n = playerInventory.GetItem() as EatItem;
            if (eatTime >= n.eatTime)
            {
                var stats = GetComponentInParent<PlayerStats>();
                if (stats.hunger < 98)
                {
                    stats.hunger += n.eatValue;
                    playerInventory.RemoveItem(n.itemName);
                }
            }
        }
    }
    public void StopEat()
    {
        itemsData.handsAnim.SetLayerWeight(1, Mathf.Clamp01(itemsData.handsAnim.GetLayerWeight(1) - Time.deltaTime));
        eatTime = 0;
    }
}

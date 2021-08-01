using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftItem {
    public Item item;
    public int count;
}
[System.Serializable]
public class Craft {
    public enum CraftType { Player, CraftingTable, Furnace, Anvil, WeaponTable};
    public CraftType craftType;
    public List<CraftItem> craftItems = new List<CraftItem>();
    public CraftItem finalItem = new CraftItem();
}


public class Crafts : MonoBehaviour
{
    public List<Craft> crafts = new List<Craft>();
}

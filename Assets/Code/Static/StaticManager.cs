using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticManager : MonoBehaviour
{
    public static StaticManager instance;
    static List<Item> allItems;
    static ResourcesPaths resourcesPaths;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            allItems = Resources.LoadAll<Item>("ItemsObjects/").ToList();
            resourcesPaths = Resources.Load<ResourcesPaths>("ResourcesPaths");


            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static Item ItemByItemName(string itemName)
    {
        return allItems.Find(x => x.itemName == itemName);
    }
    public static Item ItemByName(string itemFileName)
    {
        return allItems.Find(x => x.name == itemFileName);
    }

    public static string GetPrefabPathByName(string prefabName)
    {
        return resourcesPaths.prefabsPaths.Find(x => x.Contains(prefabName));
    }
}

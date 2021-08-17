using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Base Item", menuName = "Game/Base Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public int value, maxValue;
    public float durability;
    [Newtonsoft.Json.JsonIgnore]
    public GameObject prefab;


    public Item Clone()
    {
        var cln = Instantiate(this);
        cln.name = cln.name.Replace("(Clone)", "");
        return cln;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Base Item", menuName = "Game/Base Item", order = 1)]
public class Item : ScriptableObject
{
    public string name;
    public int value, maxValue;
    public float durability;
    public GameObject prefab;
}


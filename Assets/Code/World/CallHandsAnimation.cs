using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallHandsAnimation : MonoBehaviour
{
    public void Set()
    {
        GetComponentInParent<PlayerInventory>().Set();
    }
}

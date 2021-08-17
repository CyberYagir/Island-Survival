using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftsUIButton : MonoBehaviour
{
    public Craft craft;

    public void Click()
    {
        GetComponentInParent<CraftsUI>().DrawCraftStats(craft);
    }
}

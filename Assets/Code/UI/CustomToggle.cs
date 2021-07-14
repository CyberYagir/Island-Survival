using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomToggle : MonoBehaviour
{
    public bool isOn;
    public bool startIsNone;

    public void RevertToggle()
    {
        isOn = !isOn;
        startIsNone = false;
    }

    public void SetToggle(bool set)
    {
        isOn = set;
        startIsNone = false;
    }
}

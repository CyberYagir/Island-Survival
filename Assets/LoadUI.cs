using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadUI : MonoBehaviour
{
    public static LoadUI ui;
    public TMP_Text text;
    private void Start()
    {
        ui = this;
    }
}

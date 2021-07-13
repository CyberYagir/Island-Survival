using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LoadUI : MonoBehaviour
{
    public static LoadUI ui;
    public static int iterrations = 0;

    public static string text_p = "";
    public static string text_loading {
        get { return text_p; }
        set { 
            iterrations++;
            text_p = ("["+iterrations.ToString()+"] ") + value;  
        }
    }
    public TMP_Text text;
    private void Start()
    {
        ui = this;
    }

    private void Update()
    {
        text.text = text_loading;
    }

    public void Hide()
    {
        text.transform.parent.gameObject.SetActive(false);
        FindObjectOfType<HeadBob>().enabled = true;
    }
}

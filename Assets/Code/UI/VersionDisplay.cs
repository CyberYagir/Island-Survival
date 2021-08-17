using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class VersionDisplay : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text = "V." + Application.version;
    }
}

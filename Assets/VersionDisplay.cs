using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionDisplay : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text = "V." + Application.version;
    }
}

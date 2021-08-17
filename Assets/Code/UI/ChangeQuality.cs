using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeQuality : MonoBehaviour
{
    public void Start()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 0));
    }

    public void SetQuality(TMP_Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", dropdown.value));
        PlayerPrefs.SetInt("Quality", dropdown.value);
    }
}

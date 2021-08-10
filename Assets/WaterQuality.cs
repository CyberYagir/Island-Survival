using StylizedWater2;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaterQuality : MonoBehaviour
{
    [SerializeField] List<Material> materials;
    public void Start()
    {
        Set();
    }
    public void Set()
    {
        GetComponent<WaterObject>().AssignMaterial(materials[PlayerPrefs.GetInt("Water", 0)]);
    }
    public void SetQuality(TMP_Dropdown dropdown)
    {
        PlayerPrefs.SetInt("Water", dropdown.value);
        Set();
    }
}

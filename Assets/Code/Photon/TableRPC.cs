using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRPC : MonoBehaviourPun
{
    [PunRPC]
    public void SetGlobalResID(int resID)
    {
        StartCoroutine(set(resID));
    }

    IEnumerator set(int resID)
    {
        while (TerrainGenerator.genEnded == false) yield return null;

        var bms = FindObjectOfType<BiomesPrefabsGenerator>();
        if (bms)
        {
            while (bms.spawnedObjects.Count <= resID)
            {
                bms.spawnedObjects.Add(null);
            }
            FindObjectOfType<BiomesPrefabsGenerator>().spawnedObjects[resID] = (GetComponentInParent<Resource>().gameObject);
            GetComponentInParent<Resource>().resId = resID;
        }
    }
}

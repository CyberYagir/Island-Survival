using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemExecuter : MonoBehaviourPun
{
    [SerializeField] List<MonoBehaviour> destroys, destroyInHands;

    public void DestroysInHands()
    {
        for (int i = 0; i < destroyInHands.Count; i++)
        {
            Destroy(destroyInHands[i]);
        }
    }

    [PunRPC]
    public void Destroys()
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        for (int i = 0; i < destroys.Count; i++)
        {
            Destroy(destroys[i]);
        }
    }
}

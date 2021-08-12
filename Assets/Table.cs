using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Craft.CraftType craftType;
    public void OnTriggerStay(Collider other)
    {
        var photonView = other.GetComponentInParent<PhotonView>();
        if (photonView)
        {
            if (photonView.IsMine)
            {
                photonView.GetComponent<PlayerInventory>().AddCraftType(craftType);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var photonView = other.GetComponentInParent<PhotonView>();
        if (photonView)
        {
            if (photonView.IsMine)
            {
                photonView.GetComponent<PlayerInventory>().RemoveCraftType(craftType);
            }
        }
    }
}

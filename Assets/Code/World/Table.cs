using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Craft.CraftType craftType;

    PlayerInventory playerInventory;

    private void Start()
    {
    }

    public void OnTriggerStay(Collider other)
    {
        var photonView = other.GetComponentInParent<PhotonView>();
        if (photonView && photonView.GetComponent<PlayerInventory>() != null)
        {
            if (photonView.IsMine)
            {
                playerInventory = photonView.GetComponent<PlayerInventory>();
                playerInventory.AddCraftType(craftType);
            }
        }
    }

    private void OnDestroy()
    {
        ExitCheck();
    }

    public void OnTriggerExit(Collider other)
    {
        ExitCheck();
    }


    public void ExitCheck()
    {
        if (playerInventory != null)
        {
            playerInventory.RemoveCraftType(craftType);
            playerInventory = null;
        }
    }
}

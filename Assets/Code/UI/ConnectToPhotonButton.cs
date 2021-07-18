using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToPhotonButton : MonoBehaviour
{

    public void Click()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = false;
            PhotonLobby.lobby.InitPUN();
            FindObjectOfType<MenuUIManager>().connectToPUN.SetActive(true);
        }
    }
}

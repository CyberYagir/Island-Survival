using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectFromPhotonButton : MonoBehaviour
{
    public void Click()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonLobby.isConnectedToMasterOrLobby = false;
            PhotonNetwork.Disconnect();
        }
    }
}

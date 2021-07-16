using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectIndicator : MonoBehaviour
{
    public Image img;

    private void Update()
    {
        img.color = PhotonNetwork.IsConnected ? new Color(0, 0.5f, 1) : Color.red;
    }
}

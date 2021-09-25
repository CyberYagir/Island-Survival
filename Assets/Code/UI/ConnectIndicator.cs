using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectIndicator : MonoBehaviour
{
    Image img;
    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        img.color = PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode ? new Color(0, 0.5f, 1) : Color.red;
    }
}

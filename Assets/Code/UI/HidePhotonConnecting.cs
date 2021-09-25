using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePhotonConnecting : MonoBehaviour
{
    [SerializeField] Animator animator;
    float time;

    private void OnEnable()
    {
        time = 0;

        animator.Play("ConnectingToPhoton");
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time > 4 && PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.JoinedLobby && PhotonLobby.isConnectedToMasterOrLobby)
            animator.Play("ConnectingToPhotonOff");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

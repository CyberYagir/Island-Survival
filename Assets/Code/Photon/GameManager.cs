using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;
using ExitGames.Client.Photon;


public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static GameManager manager;
    public Player playerPrefab;

    public Player LocalPlayer;
    [HideInInspector]

    public float time;
    public static bool pause;
    private void Awake()
    {
        pause = false;
        manager = this;
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
            return;
        }
    }

    public void RespawnPlayer()
    {
        //if (!Timer.timer_.end)
            Player.RefreshInstance(ref LocalPlayer, playerPrefab, true);
    }
    private void Update()
    {
        //tabMenu.SetActive(Input.GetKey(KeyCode.Tab) || Timer.timer_.end);
        Debug.LogError("Roomname: " + PhotonNetwork.CurrentRoom.Name + "/" + PhotonNetwork.OfflineMode);
        if (LocalPlayer == null)
        {
            time += Time.deltaTime;
            if (time > 2.5f && TerrainGenerator.genEnded)
            {
                RespawnPlayer();
            }
        }

    }
    public void Disconnect()
    {
        if (LocalPlayer != null)
        {
            PhotonNetwork.Destroy(LocalPlayer.gameObject);
        }
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("Manager"));
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (otherPlayer.IsLocal)
        {
            Disconnect();
        }
        base.OnPlayerLeftRoom(otherPlayer);
    }
    void IInRoomCallbacks.OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {

    }
}

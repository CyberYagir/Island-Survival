using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine.Events;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobby lobby;
    public List<RoomInfo> rooms;
    public static string nickname;
    private void OnApplicationQuit()
    {
        ClearErrorPrefs();
    }
    private void Awake()
    {
        nickname = PlayerPrefs.GetString("Name", "Human");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        lobby = this;
    }
    public void InitPUN()
    { 
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.NickName = PlayerPrefs.GetString("Name", "Human");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            //Connected
        }
    }

    public static void ClearErrorPrefs()
    {
        PlayerPrefs.DeleteKey("Disconnect");
    }
    public void ClearError()
    {
        ClearErrorPrefs();
    }
    public void Exit()
    {
        Application.Quit();
    }

    public override void OnConnectedToMaster()
    {
        //mainUI.SetActive(true);
        base.OnConnectedToMaster();
    }


    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinLobby(new TypedLobby("DEFAULT", LobbyType.Default));
        base.OnJoinedLobby();
    }

    public void ToBattle()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void CreateRoom()
    {
        string name = "Room [" + (PhotonNetwork.CountOfRooms + 1) +  "]_";
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        RoomOptions roomOptions = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = 16, CustomRoomProperties = h};
        PhotonNetwork.CreateRoom(name, roomOptions, PhotonNetwork.CurrentLobby);
    }
    public void CreateRoom(bool visible, byte players, string seed)
    {
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("Seed", FindObjectOfType<SetStartSeed>().Format(SetStartSeed.seed));
        RoomOptions roomOptions = new RoomOptions() { IsVisible = visible, IsOpen = true, MaxPlayers = players, CustomRoomProperties = h };
        PhotonNetwork.CreateRoom(IPManager.GetIP(ADDRESSFAM.IPv4), roomOptions);
    }
    public void JoinRoom(string nm)
    {
        PhotonNetwork.JoinRoom(nm);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Failed");
        base.OnJoinRoomFailed(returnCode, message);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //errorText.text = "Join room Error";
        //CreateRoom();
        base.OnJoinRandomFailed(returnCode, message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        rooms = roomList;
        //FindObjectOfType<Rooms>().UpdateRooms();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //errorText.text = "Failed to create room";
        base.OnCreateRoomFailed(returnCode, message);
    }
}



public class IPManager
{
    public static string GetIP(ADDRESSFAM Addfam)
    {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return output;
    }
}

public enum ADDRESSFAM
{
    IPv4, IPv6
}
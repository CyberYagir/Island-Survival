using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public GameObject connectToPUN;
    public TMP_InputField nickname;
    public CustomToggle isMultiplayer;

    private void Start()
    {
        nickname.text = PhotonLobby.nickname;
    }

    public void OpenWindow(MoveWindow moveWindow)
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("OpenMenu"))
        {
            if (item.GetComponent<MoveWindow>() != moveWindow)
            {
                item.GetComponent<MoveWindow>().openClose = false;
            }
        }
        moveWindow.Toggle();
    }
    private void Update()
    {
        print(PhotonNetwork.OfflineMode);
    }

    public void SaveNickName(TMP_InputField inputField)
    {
        if (inputField.text.Trim().Length != 0)
        {
            PlayerPrefs.SetString("Name", inputField.text);
            PhotonLobby.nickname = inputField.text;
            PhotonNetwork.NickName = inputField.text;
        }
        else
        {
            inputField.text = "Human";
        }
    }


    public void Play()
    {
        if (!isMultiplayer.isOn)
        {
            PhotonNetwork.OfflineMode = true;
        }

        PhotonLobby.lobby.CreateRoom(false, 16, SetStartSeed.seed);
    }

    public void Connect(TMP_InputField field)
    {
        PhotonNetwork.OfflineMode = false;
        PhotonLobby.lobby.JoinRoom(field.text);
    }
}

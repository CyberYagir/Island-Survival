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
}

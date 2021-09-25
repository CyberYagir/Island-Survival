using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public GameObject connectToPUN;
    [SerializeField] TMP_InputField nickname, connectNickname;
    [SerializeField] CustomToggle isMultiplayer;
    [SerializeField] TMP_Text joinRoomError;
    [SerializeField] TMP_InputField ipConnect;
    public void DisplayJoinError(string error)
    {
        joinRoomError.text = $"Error: {error}";
        joinRoomError.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(hideWait());
    }

    IEnumerator hideWait()
    {
        yield return new WaitForSeconds(2);
        joinRoomError.gameObject.SetActive(false);
    }

    private void Start()
    {
        nickname.text = PhotonLobby.nickname;
        connectNickname.text = PhotonLobby.nickname;
        ipConnect.text = IPManager.GetIP(ADDRESSFAM.IPv4);
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
    public void SyncNames(TMP_InputField tmp)
    {
        nickname.text = tmp.text;
        connectNickname.text = tmp.text;
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

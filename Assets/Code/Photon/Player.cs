﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviourPun, IPunObservable
{
    public MonoBehaviour[] behaviours;
    public GameObject skin;
    private void Awake()
    {
        GameManager.pause = false;
        foreach (var item in skin.GetComponentsInChildren<Renderer>())
        {
            item.shadowCastingMode = photonView.IsMine ? UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : UnityEngine.Rendering.ShadowCastingMode.On;
        }
        transform.name = photonView.Owner.NickName + ":" + photonView.Owner.UserId;
        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].enabled = false;
            }
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
        }
        else
        {
            Dead();
        }
    }
    [PunRPC]
    public void TakeDamage(float damage, string actorName, int weapon)
    {
        if (photonView.IsMine)
        {
            //if (tank.tankOptions.hp > 0)
            //{
            //    tank.tankOptions.hp -= damage / TankModificators.defenceIncrease;
            //    if (tank.tankOptions.hp <= 0)
            //    {
            //        photonView.RPC("KillRPC", RpcTarget.All, actorName, photonView.Owner.NickName, weapon);
            //        AddDeath();
            //        foreach (var item in PhotonNetwork.CurrentRoom.Players)
            //        {
            //            if (item.Value.NickName == actorName)
            //            {
            //                var newC = new ExitGames.Client.Photon.Hashtable();
            //                newC.Add("k", ((int)item.Value.CustomProperties["k"]) + 1);
            //                newC.Add("d", (int)item.Value.CustomProperties["d"]);
            //                newC.Add("Team", (int)item.Value.CustomProperties["Team"]);
            //                item.Value.SetCustomProperties(newC);


            //                if ((string)PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == "TDM")
            //                {
            //                    var rm = new ExitGames.Client.Photon.Hashtable();
            //                    rm.Add("Mode", PhotonNetwork.CurrentRoom.CustomProperties["Mode"]);
            //                    rm.Add("Map",(int) PhotonNetwork.CurrentRoom.CustomProperties["Map"]);
            //                    rm.Add("Time", (int)PhotonNetwork.CurrentRoom.CustomProperties["Time"]);

            //                    var redK = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedKills"];
            //                    var blueK = (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueKills"];

            //                    if ((int)item.Value.CustomProperties["Team"] == 1)
            //                    {
            //                        redK++;
            //                    }
            //                    if ((int)item.Value.CustomProperties["Team"] == 2)
            //                    {
            //                        blueK++;
            //                    }

            //                    rm.Add("BlueKills", blueK);
            //                    rm.Add("RedKills", redK);
            //                    PhotonNetwork.CurrentRoom.SetCustomProperties(rm);
            //                }
            //                break;
            //            }
            //        }
            //    }
            //}
        }
    }
    public void AddDeath()
    {
        var k = (int)photonView.Owner.CustomProperties["k"];
        var d = (int)photonView.Owner.CustomProperties["d"];
        var newC = new ExitGames.Client.Photon.Hashtable();
        newC.Add("k", k);
        newC.Add("d", d + 1);
        newC.Add("Team", (int)photonView.Owner.CustomProperties["Team"]);
        photonView.Owner.SetCustomProperties(newC);
    }
    [PunRPC]
    public void KillRPC(string playerKiller, string playerKilled, int weapon)
    {
        //if (PhotonNetwork.NickName == playerKiller)
        //{
        //    WebData.playerData.exp += 15;
        //    PhotonNetwork.LocalPlayer.CustomProperties["Exp"] = WebData.playerData.exp;
        //    WebData.SaveStart();
        //}
        //KillsList.killsList.Create(playerKiller, playerKilled, weapon);
    }

    public void Dead()
    {
        if (photonView.IsMine)
        {
            //if (tank.tankOptions.hp <= 0)
            //{
            //    AddDeath();
            //    var dead = PhotonNetwork.Instantiate("TankDead", transform.position, transform.rotation);
            //    dead.GetPhotonView().RPC("Set", RpcTarget.All, tank.tankOptions.weapon, tank.tankOptions.corpus, tank.tankOptions.turretRotation, GetComponent<Rigidbody>().velocity, GetComponent<Rigidbody>().mass, GetComponent<Rigidbody>().drag, new Vector3(Random.Range(-5, 5), Random.Range(5, 20), Random.Range(-5, 10)), new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), true);
            //    dead.GetComponent<DeadTank>().StartDestroy(); 
            //    PhotonNetwork.Destroy(gameObject);
            //}
        }
    }

    public static void RefreshInstance(ref Player player, Player playerPrefab, bool withMasterClient = false)
    {
        if (PhotonNetwork.IsMasterClient == false || withMasterClient == true)
        {

            var pos = new Vector3(Random.Range(0, 2048),500, Random.Range(0, 2048));
            RaycastHit hit;
            Physics.Raycast(pos, Vector3.down, out hit);
            while (!(hit.point.y > 15 && hit.point.y < 45))
            {
                pos = new Vector3(Random.Range(0, 2048), 500, Random.Range(0, 2048));
                Physics.Raycast(pos, Vector3.down, out hit);
            }
            pos = hit.point;

            var rot = Quaternion.identity;
            if (player != null)
            {
                pos = player.transform.position;
                rot = player.transform.rotation;
                PhotonNetwork.Destroy(player.gameObject);
            }
            player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, pos, rot).GetComponent<Player>();
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(JsonUtility.ToJson(tank.tankOptions, true));
            //stream.SendNext(tank.bonuses.ToArray());
        }
        else
        {
            //tank.tankOptions = JsonUtility.FromJson<TankOptions>((string)stream.ReceiveNext());
            //tank.bonuses = ((int[])stream.ReceiveNext()).ToList();
        }
    }
}
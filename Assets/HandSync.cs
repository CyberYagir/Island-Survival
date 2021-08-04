using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSync : MonoBehaviour, IPunObservable
{
    public List<Transform> hand;
    public List<Transform> skin;
    List<Vector3> lerps = new List<Vector3>();
    private void Update()
    {
        for (int i = 0; i < skin.Count; i++)
        {
            skin[i].rotation = Quaternion.Lerp(skin[i].rotation, Quaternion.EulerAngles(lerps[i]), 20f * Time.deltaTime);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                stream.SendNext(hand[i].localEulerAngles + (i == 0 ? new Vector3(-90, 0, 0) : Vector3.zero));
            }
        }
        else
        {
            lerps = new List<Vector3>();
            for (int i = 0; i < skin.Count; i++)
            {
                lerps.Add((Vector3)stream.ReceiveNext());
            }
        }
    }
}

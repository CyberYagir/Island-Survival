using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSync : MonoBehaviourPun, IPunObservable
{
    public List<Transform> hand;
    public List<Transform> skin;
    public Transform camera, cameraBone;
    List<Quaternion> lerps = new List<Quaternion>();
    Quaternion camraRot = new Quaternion();
    private void Update()
    {
        if (!photonView.IsMine)
        {
            for (int i = 0; i < skin.Count; i++)
            {
                skin[i].localRotation = Quaternion.Lerp(skin[i].localRotation, lerps[i] * (i == 0 ? Quaternion.Euler(90, 0, 0) : Quaternion.Euler(0, 0, 0)), 20f * Time.deltaTime);
            }
            cameraBone.localRotation = Quaternion.Lerp(cameraBone.localRotation, camraRot, 10 * Time.deltaTime);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                stream.SendNext(hand[i].localRotation);
            }
            stream.SendNext(camera.localRotation);
        }
        else
        {
            lerps = new List<Quaternion>();
            for (int i = 0; i < skin.Count; i++)
            {
                lerps.Add((Quaternion)stream.ReceiveNext());
            }
            camraRot = (Quaternion)stream.ReceiveNext();
        }
    }
}

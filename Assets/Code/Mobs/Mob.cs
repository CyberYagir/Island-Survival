using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mob : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    protected float hp, speed;
    [SerializeField]
    protected Vector3 newPosition;
    [SerializeField]
    protected float movingRadius;
    [SerializeField]
    protected GameObject player;
    public bool attacked;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading) hp = (int)stream.ReceiveNext();
        else stream.SendNext(hp);
    }
    [PunRPC]
    public void TakeDamage()
    {
        hp -= 1;
    }
}

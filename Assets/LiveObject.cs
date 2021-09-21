using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiveObject : MonoBehaviourPun, IPunObservable
{
    [HideInInspector]
    public float maxHealth = 100;
    public float health = 100;


    private void Start()
    {
        maxHealth = health;
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        Destroy(Instantiate(Resources.Load<GameObject>("BloodExplosion"), transform.position, Quaternion.identity), 2);
        health -= damage;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float)stream.ReceiveNext();
        }
    }
}

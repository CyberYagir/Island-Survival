using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveObject : MonoBehaviourPun, IPunObservable
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

        if (health <= 0)
        {
            photonView.RPC("Death", RpcTarget.MasterClient);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                photonView.RPC("TakeDamage", RpcTarget.All, 10);
            }
        }
    }
    [PunRPC]
    public void SpawnParticles(){
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.enabled = false;
        }
        Destroy(Instantiate(Resources.Load<GameObject>("PoofDeath"), transform.position, Quaternion.identity), 3);
    }
    [PunRPC]
    public void Death()
    {
        if (GetComponent<Player>() != null)
        {
            if (GetComponent<PlayerMove>().enabled)
            {
                GetComponent<PlayerMove>().enabled = false;
                GetComponent<PlayerInventory>().DropAllInventory();
                GetComponent<PlayerInventory>().enabled = false;
                GetComponentInChildren<WindowManager>().CloseAll();
                GetComponentInChildren<WindowManager>().enabled = false;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 2f), 0, Random.Range(-1f, 2f)) * 5, ForceMode.Impulse);
                Destroy(GetComponentInChildren<Canvas>().gameObject);
                Instantiate(Resources.Load<GameObject>("DeathUI"), transform);
            }
        }
        else if (GetComponent<Mob>() != null)
        {
            var n = GetComponent<Mob>();
            if (n.enabled){
                n.enabled = false;
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("SpawnParticles", RpcTarget.All);
                    ChangesManager.DropObjectRPCFast(n.dropItem.name, transform.position, transform.rotation, Vector3.zero);
                    if (GetComponent<Rigidbody>() != null)
                        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 2f), 0, Random.Range(-1f, 2f)), ForceMode.Impulse);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
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

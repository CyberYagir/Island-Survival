using Newtonsoft.Json;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public Item item;
    public Transform scaler;

    public float time;

    private void Update()
    {
        time += Time.deltaTime;
    }

    [PunRPC]
    void DestroyRPC()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void InitRPC(string itemName)
    {
        item = Resources.Load<Item>("ItemsObjects/" + itemName).Clone();
        var i = Instantiate(item.prefab, scaler);

        foreach (var item in i.GetComponentsInChildren<MonoBehaviour>())
        {
            Destroy(item);
        }

        foreach (var item in i.GetComponentsInChildren<Renderer>())
        {
            if (item is MeshRenderer)
            {
                item.gameObject.layer = LayerMask.NameToLayer("Drop");
                item.gameObject.AddComponent<MeshCollider>().convex = true;
            }
        }
    }
    [PunRPC]
    void SetValue(int val)
    {
        item.value = val;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (time > 2)
        {
            if (other.GetComponentInParent<PlayerInventory>() != null)
            {
                if (other.GetComponentInParent<PlayerInventory>().gameObject.GetPhotonView().IsMine)
                {
                    other.GetComponentInParent<PlayerInventory>().AddItem(this);
                }
            }
        }
    }
}

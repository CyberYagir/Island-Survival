using Newtonsoft.Json;
using Photon.Pun;
using StylizedWater2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public Item item;
    [SerializeField] Transform scaler;

    public float time, waitTime = 4;
    [SerializeField]
    public LayerMask layerMask;

    private void Update()
    {
        time += Time.deltaTime;
        if (gameObject.GetPhotonView().IsMine) {
            if (transform.position.y < 15)
            {
                if (Physics.Raycast(transform.position + new Vector3(0, 10, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    if (transform.position.y > hit.point.y)
                    {
                        if (GetComponent<FloatingTransform>() == null)
                        {
                            GetComponent<Rigidbody>().drag = 10;
                            GetComponent<Rigidbody>().useGravity = false;
                            gameObject.AddComponent<FloatingTransform>().rollAmount = 2;
                        }
                    }
                    else
                    {
                        Destroy(GetComponent<FloatingTransform>());
                        transform.position = hit.point;
                    }
                }
            }
        }
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
    void InitRPC(string itemfilename)
    {
        item = StaticManager.ItemByName(itemfilename).Clone();
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

        if (item is PlaceItem)
        {
            i.transform.localScale = (item as PlaceItem).dropScale;
        }
    }
    [PunRPC]
    void SetValue(int val)
    {
        item.value = val;
    }

    private void OnTriggerStay(Collider other)
    {
        if (time > waitTime && this.enabled)
        {
            if (other.GetComponentInParent<PlayerInventory>() != null)
            {
                if (other.GetComponentInParent<PlayerInventory>().gameObject.GetPhotonView().IsMine)
                {
                    if (other.GetComponentInParent<PlayerInventory>().AddItem(this))
                    {
                        this.enabled = false;
                    }
                }
            }
        }
    }
}

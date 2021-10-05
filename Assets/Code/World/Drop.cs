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

    public List<PlayerInventory> triggered;
    
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

    public void OnTriggerEnter(Collider other){
        if (other.transform.parent != null)
        {
            var ph = other.transform.parent.gameObject.GetPhotonView();
            if (ph != null)
            {
                if (ph.IsMine)
                {
                    var inv = ph.GetComponentInParent<PlayerInventory>();
                    if (inv != null)
                    {
                        if (!triggered.Contains(inv)){
                            triggered.Add(inv);
                        }
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider other){
        if (other.transform.parent != null){
            var inv = other.GetComponentInParent<PlayerInventory>();
            if (triggered.Contains(inv)){
                triggered.Remove(inv);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (time > waitTime && this.enabled)
        {
            if (other.transform.parent != null)
            {
                var inv = other.GetComponentInParent<PlayerInventory>();
                if (triggered.Contains(inv)){
                    if (inv.GetComponent<LiveObject>().health > 0){
                        if (inv.AddItem(this))
                        {
                            this.enabled = false;
                            return;
                        }
                    }
                }
            }
        }
    }
}

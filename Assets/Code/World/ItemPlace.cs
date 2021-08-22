using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlace : MonoBehaviourPun
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject point;
    [SerializeField] Vector3 scale;
    [SerializeField] List<string> placeTags;
    PhotonView photonView;
    public bool dontSet;
    private void Start()
    {
        var parent = transform.parent;
        transform.parent = null;
        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
        transform.parent = parent;
        scale = transform.localScale;
        point = new GameObject("Point");
        photonView = parent.GetComponentInParent<PhotonView>();

        placeTags.Add("Ground");

        if (photonView == null)
        {
            GetComponent<PlaceItemExecuter>().Destroys();
        }
        else
        {
            GetComponent<PlaceItemExecuter>().DestroysInHands();
        }
    }

    private void OnDestroy()
    {
        Destroy(point);   
    }

    public void Spawn()
    {
        if (dontSet == false)
        {
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Resource>() == null)
                {
                    SendSpawn(point.transform.position, transform.rotation);
                }
            }
        }
        else
        {
            SendSpawn(transform.position, transform.rotation);
        }
    }

    public void SendSpawn(Vector3 pos, Quaternion rot)
    {
        var inv = GetComponentInParent<PlayerInventory>();
        ChangesManager.cm.gameObject.GetPhotonView().RPC("SpawnObject", RpcTarget.All, inv.GetItem().name, pos, rot);
        inv.RemoveItem(inv.GetItem().itemName, 1);
        Destroy(this);
    }
    public void Update()
    {
        if (photonView.IsMine)
        {
            if (dontSet) return;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5, layerMask);
            if (hit.collider == null || placeTags.Contains(hit.collider.tag) == false)
            {
                foreach (var item in GetComponentsInChildren<Renderer>())
                {
                    item.gameObject.layer = LayerMask.NameToLayer("Hands");
                }
                transform.localEulerAngles = Vector3.zero;
                transform.localScale = scale / 5f;
                point.transform.position = GetComponentInParent<PlayerInventory>().hand.transform.position;
            }
            else
            {
                foreach (var item in GetComponentsInChildren<Renderer>())
                {
                    item.gameObject.layer = LayerMask.NameToLayer("Default");
                }
                transform.localScale = scale;
                point.transform.position = hit.point;
                point.transform.localEulerAngles = new Vector3(0, -GameManager.manager.LocalPlayer.transform.localEulerAngles.y, 0);
            }
            transform.position = point.transform.position;
            var pos = GameManager.manager.LocalPlayer.transform.position;
            transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
        }
        else
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            transform.localScale = scale / 5f;
        }
    }
}

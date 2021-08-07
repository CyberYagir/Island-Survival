using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlace : MonoBehaviourPun
{
    public LayerMask layerMask;
    public GameObject point;
    public Vector3 scale;
    PhotonView photonView;
    
    private void Start()
    {
        var parent = transform.parent;
        transform.parent = null;
        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;
        transform.parent = parent;
        scale = transform.localScale;
        point = new GameObject("Point");
        photonView = GetComponentInParent<PhotonView>();
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5, layerMask))
            {
                transform.localScale = scale;
                point.transform.position = hit.point;
                point.transform.localEulerAngles = new Vector3(0, -GameManager.manager.LocalPlayer.transform.localEulerAngles.y, 0);
            }
            if (hit.collider == null)
            {
                foreach (var item in GetComponentsInChildren<Renderer>())
                {
                    item.gameObject.layer = LayerMask.NameToLayer("Default");
                }
                transform.localEulerAngles = Vector3.zero;
                transform.localScale = scale / 5f;
                point.transform.position = GetComponentInParent<PlayerInventory>().hand.transform.position;
            }
            else
            {
                foreach (var item in GetComponentsInChildren<Renderer>())
                {
                    item.gameObject.layer = LayerMask.NameToLayer("Hands");
                }
            }
            transform.position = point.transform.position;
            var pos = GameManager.manager.LocalPlayer.transform.position;
            transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
        }
        else
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.gameObject.layer = LayerMask.NameToLayer("Hands");
                transform.localScale = scale / 5f;
            }
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [HideInInspector]
    public int startHp;
    public int hp;
    public int minDropCount, maxDropCount;
    public Item item;
    public int resId;
    public List<GameObject> inTrigger;
    public AnimationCurve animationCurve;
    Vector3 scale;
    float time = 0, deadtime = 0;
    bool animate = false, dead = false;
    public void Start()
    {
        scale = transform.localScale;
        startHp = hp;
        item = item.Clone();
    }
    private void Update()
    {
        if (animate)
        {
            foreach (var item in GetComponentsInChildren<Collider>(true))
            {
                item.enabled = false;
            }
            time += Time.deltaTime * 5f;
            transform.localScale = new Vector3(scale.x + (animationCurve.Evaluate(time) / 3f), scale.y + (animationCurve.Evaluate(time) / 3f), scale.z + (animationCurve.Evaluate(time) / 3f));
            if (time > 1)
            {
                transform.localScale = scale;
                animate = false;

                foreach (var item in GetComponentsInChildren<Collider>(true))
                {
                    item.enabled = true;
                }
            }
        }
        if (dead)
        {
            if (transform.localScale.x < 0.01f) Destroy(gameObject);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 10f * Time.deltaTime);
        }
    }

    public void Dead()
    {
        dead = true;
        deadtime = 0;
    }
    public void RDestroy()
    {
        Destroy(gameObject);
    }
    public void Animate()
    {
        time = 0;
        animate = true;
    }
    public void DropResources()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < Random.Range(minDropCount, maxDropCount + 1); i++)
            {
                ChangesManager.cm.gameObject.GetPhotonView().RPC(
                    "CreateDropItem",
                    RpcTarget.All,
                    "Drop",
                    transform.position + new Vector3(0, 2, 0),
                    transform.rotation,
                    item.name, 1, Vector3.zero);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            if (!inTrigger.Contains(other.gameObject))
            {
                inTrigger.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            inTrigger.Remove(other.gameObject);
        }
    }
}

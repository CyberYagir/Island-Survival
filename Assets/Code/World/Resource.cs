using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [HideInInspector]
    public int startHp;
    [HideInInspector]
    public int hp;
    public Vector2 hpMinMax;
    public Vector2 dropCount;
    public Item item;
    public ItemExecuter.Type usableType;
    [HideInInspector]
    public int resId;
    public AnimationCurve animationCurve;

    List<GameObject> inTrigger = new List<GameObject>();
    Vector3 scale;
    float time = 0;
    bool animate = false, dead = false;
    public void Start()
    {
        hp = new System.Random(TerrainGenerator.GetSeed() + (int)transform.position.sqrMagnitude).Next((int)hpMinMax.x, (int)hpMinMax.y);
        scale = transform.localScale;
        startHp = hp;
        item = item.Clone();
    }
    private void Update()
    {
        if (animate)
        {
            time += Time.deltaTime * 5f;
            transform.localScale = new Vector3(scale.x + (animationCurve.Evaluate(time) / 3f), scale.y + (animationCurve.Evaluate(time) / 3f), scale.z + (animationCurve.Evaluate(time) / 3f));
            if (time > 1)
            {
                transform.localScale = scale;
                animate = false;
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
            for (int i = 0; i < ((dropCount.x == 1 && dropCount.y == dropCount.x) ? 1 : (int)Random.Range(dropCount.x, dropCount.y + 1)); i++)
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

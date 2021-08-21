using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RendererColor {
    public Renderer renderer;
    public List<Color> colors;
}

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

    List<RendererColor> renderers = new List<RendererColor>();
    float time = 0;
    bool animate = false, dead = false;
    public void Start()
    {
        hp = new System.Random(TerrainGenerator.GetSeed() + (int)transform.position.sqrMagnitude).Next((int)hpMinMax.x, (int)hpMinMax.y);
        
        if (GetComponent<StoneOreRandom>())
        {
            GetComponent<StoneOreRandom>().Init();
        }

        var r = GetComponentsInChildren<Renderer>().ToList().FindAll(x => x.transform.name.Contains("Quad") == false);
        foreach (var item in r)
        {
            List<Color> colors = new List<Color>();
            for (int i = 0; i < item.materials.Length; i++)
            {
                colors.Add(item.materials[i].color);
            }
            renderers.Add(new RendererColor() { renderer = item, colors = colors });
        }

        startHp = hp;
        item = item.Clone();
    }
    private void Update()
    {
        if (animate)
        {
            time += Time.deltaTime * 5f;
            foreach (var item in renderers)
            {
                var mats = item.renderer.materials;

                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i].color = item.colors[i] + (Color.red * animationCurve.Evaluate(time));
                }
                item.renderer.materials = mats;
            }
            if (time > 1)
            {
                foreach (var item in renderers)
                {
                    var mats = item.renderer.materials;

                    for (int i = 0; i < mats.Length; i++)
                    {
                        mats[i].color = item.colors[i];
                    }
                    item.renderer.materials = mats;
                }
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

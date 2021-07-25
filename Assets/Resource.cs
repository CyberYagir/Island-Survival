using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public int hp;
    public int minDropCount, maxDropCount;
    public Item item;
    public int resId;
    public List<GameObject> inTrigger;


    public void Start()
    {
        item = item.Clone();
    }

    public void RDestroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            if (!inTrigger.Contains(other.gameObject))
            {
                inTrigger.Add(other.gameObject);
                GetComponent<Animator>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            inTrigger.Remove(other.gameObject);
            if (inTrigger.Count == 0)
            {
                GetComponent<Animator>().enabled = false;
            }
        }
    }
}

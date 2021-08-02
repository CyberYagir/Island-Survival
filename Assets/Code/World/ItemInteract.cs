using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : MonoBehaviour
{

    public Resource GetResourceFromRay()
    {
        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out RaycastHit hit, 4f))
        {
            if (hit.transform != null)
            {
                Resource resource = GetResource(hit.transform.gameObject);
                return resource;
            }
        }
        return null;
    }

    public static Resource GetResource(GameObject obj)
    {
        Resource res;
        res = obj.GetComponentInParent<Resource>();
        if (res == null)
        {
            res = obj.GetComponent<Resource>();
        }
        if (res == null)
        {
            res = obj.GetComponentInChildren<Resource>();
        }

        return res;
    }
}

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
                Resource resource = GetComponentFrom<Resource>(hit.transform.gameObject);
                return resource;
            }
        }
        return null;
    }

    public LiveObject GetHealthFromRay()
    {
        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out RaycastHit hit, 4f))
        {
            if (hit.transform != null)
            {
                LiveObject resource = GetComponentFrom<LiveObject>(hit.transform.gameObject);
                return resource;
            }
        }
        return null;
    }

    public static T GetComponentFrom<T>(GameObject obj)
    {
        T res;
        res = obj.GetComponentInParent<T>();
        if (res == null)
        {
            res = obj.GetComponent<T>();
        }
        if (res == null)
        {
            res = obj.GetComponentInChildren<T>();
        }

        return res;
    }
}

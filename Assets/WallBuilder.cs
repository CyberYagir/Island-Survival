using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    private void Update()
    {
        if (GetComponent<ItemPlace>())
        {
            GetComponent<ItemPlace>().dontSet = false;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
            {
                if (hit.distance < 5)
                {
                    if (hit.transform.tag == "Ground")
                    {
                        var floor = hit.transform.GetComponentInParent<Floor>();
                        if (floor)
                        {
                            GetComponent<ItemPlace>().dontSet = true;
                            float dist = 999;
                            int id = -1;
                            for (int i = 0; i < floor.wallHolder.childCount; i++)
                            {
                                var dst = Vector3.Distance(hit.point, floor.wallHolder.GetChild(i).position);
                                if (dst < dist)
                                {
                                    dist = dst;
                                    id = i;
                                }
                            }
                            transform.position = floor.wallHolder.GetChild(id).position;
                            transform.rotation = floor.wallHolder.GetChild(id).rotation;    
                            
                        }
                    }
                }
            }
            GetComponent<ItemExecuter>().enabled = true;
        }
    }
}

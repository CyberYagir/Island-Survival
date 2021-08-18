using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBuilder : MonoBehaviour
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
                            for (int i = 0; i < floor.pointsHolder.childCount; i++)
                            {
                                var dst = Vector3.Distance(hit.point, floor.pointsHolder.GetChild(i).position);
                                if (dst < dist)
                                {
                                    dist = dst;
                                    id = i;
                                }
                            }
                            print(id);
                            transform.position = floor.pointsHolder.GetChild(id).position;
                            transform.rotation = floor.pointsHolder.GetChild(id).rotation;
                        }
                    }
                }
            }
        }
    }
}

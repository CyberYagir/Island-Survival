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
                            transform.position = floor.pointsHolder.GetChild(id).position;
                            transform.rotation = floor.pointsHolder.GetChild(id).rotation;
                            
                        }
                    }
                    if (hit.transform.tag == "Wall")
                    {
                        var wall = hit.transform.GetComponentInParent<Wall>();
                        if (wall)
                        {
                            GetComponent<ItemPlace>().dontSet = true;
                            float dist = 999;
                            int id = -1;
                            for (int i = 0; i < wall.floorPoints.childCount; i++)
                            {
                                var dst = Vector3.Distance(hit.point, wall.floorPoints.GetChild(i).position);
                                if (dst < dist)
                                {
                                    dist = dst;
                                    id = i;
                                }
                            }
                            transform.position = wall.floorPoints.GetChild(id).position;
                            transform.rotation = wall.floorPoints.GetChild(id).rotation;

                        }
                    }
                    if (Physics.Raycast(transform.position + new Vector3(0, 0.5f), Vector3.down, out RaycastHit hit2))
                    {
                        if (hit2.transform.GetComponentInParent<Floor>() && hit2.distance < 1)
                        {
                            GetComponent<ItemExecuter>().enabled = false;
                            return;
                        }
                    }
                }
            }
            GetComponent<ItemExecuter>().enabled = true;
        }
    }
}

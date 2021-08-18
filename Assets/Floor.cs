using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Transform pointsHolder;
    [SerializeField] GameObject mesh;
    [SerializeField] bool onGround;
    [SerializeField] bool init;
    private void Update()
    {
        if (!init)
        {
            if (!GetComponent<ItemExecuter>())
            {
                if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit))
                {
                    if (hit.transform.GetComponent<Terrain>())
                    {
                        Spawn();
                    }
                }
                else
                {
                    Spawn();
                }
            }
        }
    }

    public void Spawn()
    {
        var setLength = Vector3.Distance(transform.position, new Vector3(transform.position.x, 0, transform.position.z));
        var n = Instantiate(mesh, new Vector3(mesh.transform.position.x, transform.position.y / 2f, mesh.transform.position.z), mesh.transform.rotation, transform);

        n.transform.localScale = new Vector3(mesh.transform.localScale.x, mesh.transform.localScale.y, setLength * mesh.transform.localScale.x * 3.2f);

        init = true;
    }
}
 
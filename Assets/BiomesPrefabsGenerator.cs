using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesPrefabsGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Prefabs {
        public Color color;
        public List<Prefab> prefabs;
    }
    [System.Serializable]
    public class Prefab {
        public GameObject prefab;
        public int chance;
        public Vector2 range;


        public bool RandIs()
        {
            return Random.Range(0, chance) <= 1;
        }
    }
    public int objectsCount;
    public int objects;
    public List<Prefabs> biomeObject;
    public List<Vector2> poses;

    public void GenPrefabs()
    {
        var terr = GetComponent<Terrain>();
        while (objects < objectsCount)
        {
            for (int q = 0; q < biomeObject.Count; q++)
            {
                for (int o = 0; o < biomeObject[q].prefabs.Count; o++)
                {
                    if (biomeObject[q].prefabs[o].RandIs())
                    {
                        var pos = new Vector2(Random.Range(0, terr.terrainData.alphamapResolution), Random.Range(0, terr.terrainData.alphamapResolution));
                        if (!poses.Contains(pos))
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(new Vector3(pos.x, 500, pos.y), Vector3.down, out hit))
                            {
                                if (hit.point.y > biomeObject[q].prefabs[o].range.x && hit.point.y < biomeObject[q].prefabs[o].range.y)
                                {
                                    Instantiate(biomeObject[q].prefabs[o].prefab, hit.point, Quaternion.identity);
                                    objects++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

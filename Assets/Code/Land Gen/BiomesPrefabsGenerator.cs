using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BiomesPrefabsGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Prefabs
    {
        public Color color;
        public List<Prefab> prefabs;
    }
    [System.Serializable]
    public class Prefab
    {
        public GameObject prefab;
        public Vector2 range;
        public int chance;
        public bool rot;

    }
    public int objectsCount;
    public int objects;
    public List<Prefabs> biomeObject;
    public List<Vector2> poses;
    public BiomesFormatter biomesFormatter;
    public float scale;
    public TextureCreator textureCreator;
    public List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        textureCreator.FillTexture();
    }

    public void GenPrefabs()
    {
        var rnd = new System.Random(GetComponent<TerrainGenerator>().seed);
        var terr = GetComponent<Terrain>();

        var holder = new GameObject() { name = "Resources Holder" };
        for (int x = 0; x < terr.terrainData.alphamapResolution; x++)
        {
            for (int y = 0; y < terr.terrainData.alphamapResolution; y++)
            {
                for (int q = 0; q < biomeObject.Count; q++)
                {
                    for (int o = 0; o < biomeObject[q].prefabs.Count; o++)
                    {
                        var val = rnd.Next(0, biomeObject[q].prefabs[o].chance * 100);
                        if (val <= 1 && !poses.Contains(new Vector2(x, y)))
                        {
                            var posInPercents = new Vector2((float)x / (float)terr.terrainData.alphamapResolution, (float)y / (float)terr.terrainData.alphamapResolution);
                            if (biomeObject[q].color == biomesFormatter.texture2D.GetPixel((int)(biomesFormatter.biomeTexSize * posInPercents.x), (int)(biomesFormatter.biomeTexSize * posInPercents.y)))
                            {
                                RaycastHit hit;
                                if (Physics.Raycast(new Vector3(x, 500, y), Vector3.down, out hit))
                                {
                                    if (hit.point.y > biomeObject[q].prefabs[o].range.x && hit.point.y < biomeObject[q].prefabs[o].range.y)
                                    {
                                        if (Vector3.Angle(hit.normal, Vector3.up) < 15)
                                        {
                                            var n = Instantiate(biomeObject[q].prefabs[o].prefab, hit.point, Quaternion.identity, holder.transform);
                                            n.transform.localEulerAngles = new Vector3(0, biomeObject[q].prefabs[o].rot ? rnd.Next(0, 360) : 0, 0);
                                            spawnedObjects.Add(n.gameObject);
                                            var res = n.GetComponent<Resource>();
                                            if (res != null)
                                            {
                                                res.resId = spawnedObjects.Count - 1;
                                            }

                                            poses.Add(new Vector2(x, y));
                                            objects++;
                                            if (objects >= objectsCount)
                                            {
                                                return;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        LoadUI.ui.Hide();
        TerrainGenerator.genEnded = true;
    }
}
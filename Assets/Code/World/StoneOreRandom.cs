using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneOreRandom : MonoBehaviour
{
    [SerializeField] List<Item> ores = new List<Item>();
    [SerializeField] List<Material> materials = new List<Material>();
    [SerializeField] Renderer oreMesh;
    public void Init()
    {
        var gen = FindObjectOfType<TerrainGenerator>();
        if (gen != null)
        {
            System.Random rnd = new System.Random((int)(TerrainGenerator.GetSeed() + transform.position.sqrMagnitude));
            var isOre = rnd.Next(0, 4);
            if (isOre == 0)
            {
                oreMesh.gameObject.SetActive(true);
                var id = rnd.Next(0, ores.Count);
                GetComponent<Resource>().item = ores[id].Clone();
                oreMesh.material = materials[id];
            }
            else
            {
                oreMesh.gameObject.SetActive(false);
            }
        }
    }
}

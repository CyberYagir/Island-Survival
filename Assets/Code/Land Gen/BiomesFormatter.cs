using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesFormatter : MonoBehaviour
{
    public int biomeTexSize;

    public List<TextureCreator> biomes = new List<TextureCreator>();
    public List<Color> colors = new List<Color>();
    public Texture2D texture2D;
    public event Action OnEndGenBiomes;

    public void FormateBiomes()
    {
        texture2D = new Texture2D(biomeTexSize, biomeTexSize);
        texture2D.filterMode = FilterMode.Point;
        var t = FindObjectOfType<TerrainGenerator>();
        int id = 2;

        List<TextureCreator> creators = new List<TextureCreator>();

        foreach (var item in biomes)
        {
            id++;
            item.offcet = Mathf.PerlinNoise(id + Mathf.Sqrt(t.main.offcet.x), id + Mathf.Sqrt(t.main.offcet.y)) * (-t.seed * (id % 2 == 0 ? 1 : -1)) * Vector3.one * id;
            item.FillTexture();
            creators.Add(item);
            item.OnEndTexture += () =>
            {
                creators.Remove(item);
                if (creators.Count == 0)
                {
                    Color32[] cls = new Color32[texture2D.width * texture2D.height];
                    for (int x = 0; x < texture2D.width; x++)
                    {
                        for (int y = 0; y < texture2D.height; y++)
                        {
                            Color color = Color.black;
                            for (int i = 0; i < biomes.Count; i++)
                            {
                                if (((Color)biomes[i].colors[y * texture2D.height + x]).grayscale > 0.01f)
                                {
                                    color = colors[i];
                                    break;
                                }
                            }
                            cls[y * texture2D.height + x] = color;
                        }
                    }
                    texture2D.SetPixels32(cls);
                    texture2D.Apply();
                    OnEndGenBiomes();
                }
            };
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesFormatter : MonoBehaviour
{
    public class ColorsCounter
    {
        public Color32 color;
        public int count;
    }
    public int biomeTexSize;
    [SerializeField]
    List<TextureCreator> biomes = new List<TextureCreator>();
    public List<Color> colors = new List<Color>();
    public Texture2D texture2D;
    public event Action OnEndGenBiomes;
    bool started;
    List<TextureCreator> creators = new List<TextureCreator>();
    Vector2 offcet;
    System.Random rnd;
    private void Start()
    {
        rnd = new System.Random(TerrainGenerator.GetSeed());
    }

    private void Update()
    {
        if (started)
        {
            if (creators.Count == 0 && texture2D != null)
            {
                List<ColorsCounter> colors = new List<ColorsCounter>();
                for (int x = 0; x < texture2D.width; x++)
                {
                    for (int y = 0; y < texture2D.height; y++)
                    {
                        var find = colors.Find(z => z.color == texture2D.GetPixel(x, y));
                        if (find != null)
                        {
                            find.count++;
                        }
                        else
                        {
                            colors.Add(new ColorsCounter() { color = texture2D.GetPixel(x, y), count = 1 });
                        }
                    }
                }
                foreach (var item in colors)
                {
                    if ((item.count / (float)(texture2D.width * texture2D.height)) > 0.6f)
                    {
                        offcet += new Vector2(rnd.Next(-100, 100), rnd.Next(-100, 100));
                        FormateBiomes();
                        return;
                    }
                }
                OnEndGenBiomes();
                OnEndGenBiomes = delegate { };
                started = false;
            }
        }
    }

    public void FormateBiomes()
    {
        started = true;
        texture2D = new Texture2D(biomeTexSize, biomeTexSize, TextureFormat.RGB24, true);
        texture2D.filterMode = FilterMode.Point;
        var t = FindObjectOfType<TerrainGenerator>();
        int id = 2;


        foreach (var item in biomes)
        {
            id++;
            item.offcet = Mathf.PerlinNoise(id + Mathf.Sqrt(t.main.offcet.x) + offcet.x, id + Mathf.Sqrt(t.main.offcet.y) + offcet.y) * (-TerrainGenerator.GetSeed() * (id % 2 == 0 ? 1 : -1)) * Vector3.one * id;
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
                }
            };  
        }
    }
}

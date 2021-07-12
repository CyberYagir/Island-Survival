using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;
    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;
    public int octaves;
    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F, persistance, lacunar;

    public Texture2D noiseTex;
    private Color[] pix;

    public int seed;

    public TextureCreator main, moutains;

    public Color color;

    public AnimationCurve shoothRamp;

    public List<TextureCreator> waitForTextures;


    void Start()
    {
        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
    }

    public Texture2D _CalcNoise()
    {

        float maxNoise = float.MinValue;
        float minNoise = float.MaxValue;
        var n = new float[pixWidth, pixHeight];
        for (float y = 0; y < noiseTex.height; y++)
        {
            for (float x = 0; x < noiseTex.width; x++)
            {
                float amplitude = 1;
                float freq = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (((pixWidth) + (x / noiseTex.width) * (scale * 2)) + xOrg) * freq;
                    float yCoord = (((pixHeight) + (y / noiseTex.height) * (scale * 2)) + yOrg) * freq;

                    float sample = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;

                    noiseHeight += sample * amplitude;
                    amplitude *= persistance;
                    freq *= lacunar;
                }

                if (noiseHeight > maxNoise)
                {
                    maxNoise = noiseHeight;
                }
                if (noiseHeight < minNoise)
                {
                    minNoise = noiseHeight;
                }
                if (distance_squared(x, y) < 0.5f)
                {
                    pix[(int)y * noiseTex.width + (int)x] = new Color(noiseHeight, noiseHeight, noiseHeight) * (0.8f - distance_squared(x, y));
                    n[(int)y, (int)x] = noiseHeight * (0.8f - distance_squared(x, y));
                }
            }
        }
        GetComponent<Terrain>().terrainData.SetHeights(0, 0, n);
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        return noiseTex;
    } //NO WHILE
    float distance_squared(float x, float y)
    {
        var dx = 2 * x / pixWidth - 1;
        var dy = 2 * y / pixHeight - 1;
        // at this point 0 <= dx <= 1 and 0 <= dy <= 1
        return dx * dx + dy * dy;
    }
    bool pressed;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && !pressed)
        {
            waitForTextures.Add(main);
            waitForTextures.Add(moutains);
            main.FillTexture();
            moutains.FillTexture();
            main.OnEndTexture += () =>
            {
                waitForTextures.Remove(main);
                if (waitForTextures.Count == 0)
                {
                    Draw();
                }
            };

            moutains.OnEndTexture += () =>
            {
                waitForTextures.Remove(moutains);
                if (waitForTextures.Count == 0)
                {
                    Draw();
                }
            };


        }
    }

    public void Draw()
    {
        noiseTex = main.texture;
        var mouTex = moutains.texture;

        var n = new float[pixWidth, pixHeight];
        for (int x = 0; x < noiseTex.width; x++)
        {
            for (int y = 0; y < noiseTex.height; y++)
            {
                var max = 0.5f;
                if (distance_squared(x, y) < max)
                {
                    n[x, y] = ((noiseTex.GetPixel(x, y).grayscale * 0.15f)) + ((mouTex.GetPixel(x, y).grayscale) * 0.8f);
                }
                else
                {
                    n[x, y] = ((noiseTex.GetPixel(x, y).grayscale * 0.15f) + (mouTex.GetPixel(x, y).grayscale * 0.8f)) * shoothRamp.Evaluate((1 - ((distance_squared(x, y) - max) / (1 - max))));
                }
            }
        }
        GetComponent<Terrain>().terrainData.SetHeights(0, 0, n);
        GetComponent<TerrainTexturer>().DrawTextureTerrain();
        GetComponent<DetailGenerator>().GenGrass();
        GetComponent<BiomesFormatter>().FormateBiomes();
        GetComponent<BiomesFormatter>().OnEndGenBiomes += () => { GetComponent<BiomesPrefabsGenerator>().GenPrefabs(); };
    }
}

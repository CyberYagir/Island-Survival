using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TextureCreator : MonoBehaviour
{

    [Range(2, 2048)]
    public int resolution = 256;

    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Range(1, 3)]
    public int dimensions = 3;
    public Vector3 offcet;
    public NoiseMethodType type;

    public Gradient coloring;

    public Texture2D texture;


    public event Action OnEndTexture = delegate { };

    Task thread;
    [HideInInspector]
    public Color32[] colors;

    private void Init()
    {
        if (texture == null)
        {
            texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
            texture.name = "Procedural Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 9;
            if (GetComponent<MeshRenderer>())
                GetComponent<MeshRenderer>().material.mainTexture = texture;

            //FillTexture();
        }

    }

    private void Update()
    {
        if (thread != null)
        {
            if (thread.Exception != null)
            {
                foreach (var item in thread.Exception.InnerExceptions)
                {
                    //print(item.Message);
                }
            }
            if (thread.IsCompleted)
            {
                Init();
                texture.SetPixels32(colors);
                texture.Apply();
                OnEndTexture();
                thread = null;
            }            
        }
    }



    public void FillTexture()
    {

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        thread = new Task(() =>
        {

            colors = new Color32[resolution * resolution];

            NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
            float stepSize = 1f / resolution;

            for (int y = 0; y < resolution; y++)
            {
                Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
                Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
                for (int x = 0; x < resolution; x++)
                {
                    Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                    float sample = Noise.Sum(method, point + offcet, frequency, octaves, lacunarity, persistence);
                    if (type != NoiseMethodType.Value)
                    {
                        sample = sample * 0.5f + 0.5f;
                    }
                    colors[y * resolution + x] = coloring.Evaluate(sample);
                }
            }
        });
        thread.Start();
    }
}
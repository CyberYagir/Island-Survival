using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailGenerator : MonoBehaviour
{
    public int patchDetail, grassDensity;
    public void GenGrass()
    {
        var terrainToPopulate = transform.gameObject.GetComponent<Terrain>();
        terrainToPopulate.terrainData.SetDetailResolution(grassDensity, patchDetail);
        var terrainData = terrainToPopulate.terrainData;
        int[,] newMap = new int[terrainToPopulate.terrainData.alphamapWidth, terrainToPopulate.terrainData.alphamapHeight];

        for (int x = 0; x < terrainToPopulate.terrainData.alphamapWidth; x++)
        {
            for (int y = 0; y < terrainToPopulate.terrainData.alphamapHeight; y++)
            {
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;
                Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution), Mathf.RoundToInt(x_01 * terrainData.heightmapWidth));

                if (height > 20 && height < 50 && Vector3.Angle(normal, Vector3.up) < 40)
                {
                    newMap[x, y] = 1;
                }
            }
        }
        terrainToPopulate.terrainData.SetDetailLayer(0, 0, 0, newMap);

    }
}

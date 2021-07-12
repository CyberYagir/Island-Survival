using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainTexturer : MonoBehaviour
{
    public void DrawTextureTerrain()
    {
        // Get the attached terrain component
        Terrain terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        TerrainData terrainData = terrain.terrainData;
        //terrainData.heightmapResolution = 2049;
        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1 
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapResolution), Mathf.RoundToInt(x_01 * terrainData.heightmapResolution));

                // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
                Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

                // Calculate the steepness of the terrain
                float steepness = terrainData.GetSteepness(y_01, x_01);

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                if (height < 50 && Vector3.Angle(normal, Vector3.up) < 40)
                {
                    if (height > 22)
                    {
                        splatWeights[0] = 0;
                        splatWeights[1] = 1;
                        splatWeights[2] = 0;
                    }
                    else if (height < 20)
                    {
                        splatWeights[0] = 1;
                        splatWeights[1] = 0;
                        splatWeights[2] = 0;
                    }
                    else
                    {
                        splatWeights[0] = 1f - ((height - 20) / (22f - 20f));
                        splatWeights[1] = ((height - 20) / (22f - 20f));
                        splatWeights[2] = 0;
                        //n = (height / 22f);
                    }
                }
                else if ((height > 60) || Vector3.Angle(normal, Vector3.up) > 40)
                {
                    splatWeights[0] = 0;
                    splatWeights[1] = 0;
                    splatWeights[2] = 1;
                }
                else
                {
                    splatWeights[0] = 0;
                    splatWeights[1] = 1f - ((height - 50) / (60 - 50f)); 
                    splatWeights[2] = ((height - 50) / (60 - 50f));
                }
                if (height > 160f)
                {
                    splatWeights[3] = (height-160f)/(170f-160f);
                }
                // CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

                // Texture[0] has constant influence



                //splatWeights[0] = 0f;

                // Texture[1] is stronger at lower altitudes
                //splatWeights[1] = Mathf.Clamp01((terrainData.heightmapResolution - height));

                //// Texture[2] stronger on flatter terrain
                //// Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
                //// Subtract result from 1.0 to give greater weighting to flat surfaces
                ///splatWeights[2] = 1.0f - Mathf.Clamp01(steepness * steepness / (terrainData.heightmapHeight / 5.0f));

                //// Texture[3] increases with height but only on surfaces facing positive Z axis 
                //splatWeights[3] = height * Mathf.Clamp01(normal.z);

                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                // Loop through each terrain texture
                for (int i = 0; i < splatWeights.Length; i++)
                {

                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // Assign this point to the splatmap array
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }

        // Finally assign the new splatmap to the terrainData:
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
}

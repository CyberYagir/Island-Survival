using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
[ExecuteAlways]
public class StaticManagerEditor : MonoBehaviour
{
    ResourcesPaths resourcesPaths;
    List<string> prefabs, items;
    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            prefabs = new List<string>();
            items = new List<string>();
            DirSearch(Application.dataPath + "/Resources/");
            if (resourcesPaths == null)
            {
                resourcesPaths = Resources.Load<ResourcesPaths>("ResourcesPaths");
            }
            else
            {
                if (!resourcesPaths.prefabsPaths.SequenceEqual(prefabs) || !resourcesPaths.itemsPaths.SequenceEqual(items))
                {
                    print("Paths Updated!");
                    resourcesPaths.prefabsPaths = prefabs;
                    resourcesPaths.itemsPaths = items;
                    AssetDatabase.SaveAssets();
                }
        }
            }
    }


    void DirSearch(string sDir)
    {
        foreach (string d in Directory.GetDirectories(sDir))
        {
            foreach (string f in Directory.GetFiles(d))
            {
                var withoutEx = f.Split('.')[0];
                var final = withoutEx.Remove(0, withoutEx.IndexOf("Resources/")).Replace("Resources/","");
                switch (Path.GetExtension(f))
                {
                    case ".prefab":
                        prefabs.Add(final);
                        break;
                    case ".asset":
                        items.Add(final);
                        break;
                }
            }
            DirSearch(d);
        }
    }
}

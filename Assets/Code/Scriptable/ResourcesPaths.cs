using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcesPaths", menuName = "Game/Resources Paths", order = 4)]
public class ResourcesPaths : ScriptableObject
{
    public List<string> prefabsPaths, itemsPaths;
}

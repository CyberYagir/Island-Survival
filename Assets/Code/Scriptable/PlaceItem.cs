using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PreviewCustom Item", menuName = "Game/Preview Custom Item", order = 1)]
public class PlaceItem : Item
{
    [Newtonsoft.Json.JsonIgnore]
    public Vector3 previewScale, previewRot, previewOffcet;
}

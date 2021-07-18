using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRenderItems : MonoBehaviour
{
    public Camera camera;
    public List<RenderTexture> textures;
    public PlayerInventory playerInventory;
    public Transform point;
    public void Update()
    {
        point.localEulerAngles += new Vector3(0, Time.deltaTime * 20f, 0);
    }
    private void FixedUpdate()
    {
        Set();
    }
    public void Init()
    {
        textures = new List<RenderTexture>(new RenderTexture[playerInventory.items.Count]);
        for (int i = 0; i < textures.Count; i++)
        {
            textures[i] = new RenderTexture(256, 256, 0);
        }
    }
    private void Start()
    {
        Init();
    }

    public void Set()
    {
        Init();
        for (int i = 0; i < textures.Count; i++)
        {
            if (playerInventory.items[i] != null)
            {
                var n = Instantiate(playerInventory.items[i].prefab, point);
                foreach (Transform item in n.transform)
                {
                    item.gameObject.layer = LayerMask.NameToLayer("Inventory");
                }
                ClearOutRenderTexture(camera.targetTexture);
                ClearOutRenderTexture(textures[i]);
                camera.targetTexture = textures[i];
                camera.Render();
                Destroy(n.gameObject);
            }
        }
    }
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }

}

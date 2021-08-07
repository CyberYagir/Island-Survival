using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRenderItems : MonoBehaviour
{
    public Camera camera;
    public List<RenderTexture> textures;
    public PlayerInventory playerInventory;
    public List<GameObject> gameObjects;
    public RenderTexture other;
    public Transform point;
    public static InventoryRenderItems inventoryRender;
    
    public void Update()
    {
        point.localEulerAngles += new Vector3(0, Time.deltaTime * 20f, 0);
    }
    private void FixedUpdate()
    {
        Set(false);
    }
    public void Init()
    {
        textures = new List<RenderTexture>(new RenderTexture[playerInventory.items.Count]);
        for (int i = 0; i < gameObjects.Count; i++)
        {
            Destroy(gameObjects[i].gameObject);
        }
        gameObjects = new List<GameObject>();
        //print("Destroy");
        for (int i = 0; i < textures.Count; i++)
        {
            textures[i] = new RenderTexture(128, 128, 0);
            if (playerInventory.items[i] != null)
            {
                var obj = Instantiate(playerInventory.items[i].prefab, point);
                obj.transform.GetChild(0).localPosition = Vector3.zero;
                gameObjects.Add(obj);
            }
        }
    }

    public void ResetDisplay()
    {
        Init();
        Set();
    }
    private void Start()
    {
        inventoryRender = this;
        other = new RenderTexture(512, 512, 0);
        ResetDisplay();
    }

    public RenderTexture render(GameObject item)
    {
        var obj = Instantiate(item.gameObject, point);
        obj.transform.GetChild(0).localPosition = Vector3.zero;
        foreach (Transform it in obj.transform)
        {
            it.gameObject.layer = LayerMask.NameToLayer("Inventory");
        }

        camera.targetTexture = other;
        camera.RenderDontRestore();
        Destroy(obj);
        return other;
    }

    public void Set(bool check = true)
    {
        try
        {
            for (int i = 0; i < textures.Count; i++)
            {
                if (i == playerInventory.selected || check)
                {
                    if (playerInventory.items[i] != null)
                    {
                        gameObjects[i].SetActive(true);
                        foreach (Transform item in gameObjects[i].transform)
                        {
                            item.gameObject.layer = LayerMask.NameToLayer("Inventory");
                        }
                        camera.targetTexture = textures[i];
                        camera.RenderDontRestore();
                        gameObjects[i].SetActive(false);
                    }
                    else
                    {
                        textures[i].Release();
                    }
                }
            }
        }
        catch (Exception)
        {
            Init();
        }
    }

}

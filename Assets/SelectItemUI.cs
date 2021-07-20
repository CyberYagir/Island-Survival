using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public int id;
    public RectTransform rectTransform, line;
    public float addX;
    public Vector2 startX;
    public InventoryRenderItems inventoryRender;
    public RawImage rawImage;
    private void Start()
    {
        startX = rectTransform.anchoredPosition;
        GetComponentInChildren<TMP_Text>().text = (id+1).ToString();
    }

    private void Update()
    {
        if (playerInventory.items[id])
        {
            line.parent.gameObject.SetActive(true);
            line.localScale = new Vector3(playerInventory.items[id].durability, 1, 1);
        }
        else
        {
            line.parent.gameObject.SetActive(false);
        }
        rawImage.texture = inventoryRender.textures[id];
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, startX + (playerInventory.selected == id ? new Vector2(addX, 0) : new Vector2(0, 0)), 5 * Time.deltaTime);
    }

}

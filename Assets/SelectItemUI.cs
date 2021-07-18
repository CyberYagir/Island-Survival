using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public int id;
    public RectTransform rectTransform;
    public float addX;
    public Vector2 startX;
    public InventoryRenderItems inventoryRender;
    public RawImage rawImage;
    private void Start()
    {
        startX = rectTransform.anchoredPosition;
        GetComponentInChildren<TMP_Text>().text = id.ToString();
    }

    private void Update()
    {
        rawImage.texture = inventoryRender.textures[id];
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, startX + (playerInventory.selected == id ? new Vector2(addX, 0) : new Vector2(0, 0)), 5 * Time.deltaTime);
    }

}

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
    public TMP_Text id_text, count_text;
    private void Start()
    {
        startX = rectTransform.anchoredPosition;
        id_text.text = (id+1).ToString();
    }

    private void Update()
    {
        if (playerInventory.items[id])
        {
            line.parent.gameObject.SetActive(true);
            if (playerInventory.items[id].value > 1)
            {
                count_text.text = "x" + playerInventory.items[id].value;
            }
            else
                count_text.text = "";
            line.localScale = new Vector3(playerInventory.items[id].durability, 1, 1);
        }
        else
        {
            count_text.text = "";
            line.parent.gameObject.SetActive(false);
        }
        rawImage.texture = inventoryRender.textures[id];
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, startX + (playerInventory.selected == id ? new Vector2(addX, 0) : new Vector2(0, 0)), 5 * Time.deltaTime);
    }

}

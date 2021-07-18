using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItemUI : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public int id;
    public RectTransform rectTransform;
    public float addX;
    public Vector2 startX;

    private void Start()
    {
        startX = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, startX + (playerInventory.selected == id ? new Vector2(addX, 0) : new Vector2(0, 0)), 5 * Time.deltaTime);
    }
}

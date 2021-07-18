using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindow : MonoBehaviour
{
    public Vector2 openPos, hidedPos;
    public bool openClose;
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, openClose ? openPos : hidedPos, 8 * Time.deltaTime);
    }
    public void SetOpen(bool set)
    {
        openClose = set;
    }

    public void Toggle()
    {
        print("toggle");
        openClose = !openClose;
    }
}

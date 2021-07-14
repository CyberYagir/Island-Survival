using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool over;
    RectTransform rect;
    Vector2 startPos;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        startPos = new Vector2(rect.anchoredPosition.x, -131f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }

    private void Update()
    {
        if (over)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(rect.anchoredPosition.x, -10), 5 * Time.deltaTime);
        }
        else
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(rect.anchoredPosition.x, startPos.y), 5 * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimateBack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonAnimate buttonAnimate;



    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonAnimate.over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonAnimate.over = false;
    }
}

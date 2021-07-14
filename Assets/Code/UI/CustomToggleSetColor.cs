using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggleSetColor : MonoBehaviour
{
    public Color onColor, offColor;
    public CustomToggle customToggle;
    public Image image;
    Color curr;

    private void Start()
    {
        if (!customToggle.startIsNone)
        {
            curr = customToggle.isOn ? onColor : offColor;
        }
        else
        {
            curr = Color.gray;
        }
    }

    private void Update()
    {
        if (!customToggle.startIsNone)
        {
            curr = customToggle.isOn ? onColor : offColor;
        }
        else
        {
            curr = Color.gray;
        }

        image.color = Color.Lerp(image.color, curr, 10 * Time.deltaTime);
    }

}

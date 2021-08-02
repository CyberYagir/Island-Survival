using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public TMP_Text text;
    public GameObject bar;
    public Image value;
    private void Update()
    {
        text.gameObject.SetActive(false);
        bar.SetActive(false);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 4f))
        {
            if (hit.transform != null)
            {
                Resource resource = ItemInteract.GetResource(hit.transform.gameObject);
                if (resource)
                {
                    text.gameObject.SetActive(true);
                    text.text = resource.item.itemName; bar.SetActive(true);
                    value.fillAmount = (float)resource.hp / (float)resource.startHp;
                    //text.transform.position = Vector3.Lerp(text.transform.position, hit.point, 10f * Time.deltaTime);
                }
            }
        }
    }
}

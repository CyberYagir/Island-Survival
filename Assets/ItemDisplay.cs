using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public TMP_Text text;
    private void Update()
    {
        text.gameObject.SetActive(false);
        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out RaycastHit hit, 4f))
        {
            if (hit.transform != null)
            {
                Resource resource = ItemInteract.GetResource(hit.transform.gameObject);
                if (resource)
                {
                    print("ok");
                    text.gameObject.SetActive(true);
                    text.text = resource.item.itemName;
                    //text.transform.position = Vector3.Lerp(text.transform.position, hit.point, 10f * Time.deltaTime);
                }
            }
        }
    }
}

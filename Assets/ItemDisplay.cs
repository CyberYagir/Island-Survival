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
                Resource resource = hit.transform.GetComponentInParent<Resource>();
                if (resource == null)
                {
                    resource = hit.transform.GetComponent<Resource>();
                }
                if (resource)
                {
                    text.gameObject.SetActive(true);
                    text.text = resource.item.name;
                    text.transform.position = Vector3.Lerp(text.transform.position, Camera.main.WorldToScreenPoint(hit.point), 10f * Time.deltaTime);
                }
            }
        }
    }
}

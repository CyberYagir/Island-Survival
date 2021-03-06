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
    Interactable old;
    private void Update()
    {
        if (old != null)
        {
            old.inOver = false;
        }
        text.gameObject.SetActive(false);
        bar.SetActive(false);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 4f))
        {
            if (hit.transform != null)
            {
                Resource resource = ItemInteract.GetComponentFrom<Resource>(hit.transform.gameObject);
                if (resource)
                {
                    text.gameObject.SetActive(true);
                    text.text = resource.item.itemName; 
                    bar.SetActive(true);
                    value.fillAmount = (float)resource.hp / (float)resource.startHp;
                }
                else if (ItemInteract.GetComponentFrom<LiveObject>(hit.transform.gameObject) != null)
                {
                    var live = ItemInteract.GetComponentFrom<LiveObject>(hit.transform.gameObject);
                    text.gameObject.SetActive(true);
                    text.text = live.transform.name.Split(':')[0];
                    bar.SetActive(true);
                    value.fillAmount = (float)live.health / (float)live.maxHealth;
                }
                else
                {
                    var interact = hit.transform.GetComponentInParent<Interactable>();
                    if (interact != null)
                    {
                        text.gameObject.SetActive(true);
                        text.text = "\"F\" to " + interact._interactText;
                        interact.inOver = true;
                        old = interact;
                    }
                }
            }
        }
    }
}

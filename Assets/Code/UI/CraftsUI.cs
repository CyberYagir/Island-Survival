using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftsUI : MonoBehaviour
{
    public Transform holder;
    public GameObject item;
    public GameObject main;
    public RawImage image;
    public TMP_Text itemName;
    MoveWindow moveWindow;
    [HideInInspector]
    Craft selected = null;
    [Space]
    public GameObject subHolder;
    public GameObject subItem;


    private void Start()
    {
        moveWindow = GetComponent<MoveWindow>();
        main.SetActive(false);
        DrawCrafts();
    }
    private void Update()
    {
        if (selected != null && moveWindow.openClose == true)
        {
            image.texture = FindObjectOfType<InventoryRenderItems>().render(selected.finalItem.item.prefab);
        }
    }

    public void DrawCraftStats(Craft craft)
    {
        main.SetActive(true);
        itemName.text = craft.finalItem.item.itemName;
        selected = craft;

        foreach (Transform item in subHolder.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < craft.craftItems.Count; i++)
        {
            var n = Instantiate(subItem, subHolder.transform);
            n.transform.GetChild(0).GetComponent<TMP_Text>().text = craft.craftItems[i].item.itemName;
            var it = GetComponentInParent<PlayerInventory>().items.Find(x => x != null && x.itemName == craft.craftItems[i].item.itemName);
            n.transform.GetChild(1).GetComponent<TMP_Text>().text = (it == null ? "0" : it.value.ToString()) + "/" + craft.craftItems[i].count;
            n.SetActive(true);
        }
    }

    public void DrawCrafts()
    {
        var crafts = FindObjectOfType<Crafts>().crafts;
        for (int i = 0; i < crafts.Count; i++)
        {
            if (GetComponentInParent<PlayerInventory>().craftTypes.Contains(crafts[i].craftType))
            {
                var n = Instantiate(item, holder);
                n.GetComponent<CraftsUIButton>().craft = crafts[i];
                n.SetActive(true);
                n.GetComponentInChildren<TMP_Text>().text = crafts[i].finalItem.item.itemName;
            }
        }
    }
}

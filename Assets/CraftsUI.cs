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
    public Image image;
    public TMP_Text itemName;
    private void Start()
    {
        main.SetActive(false);
        DrawCrafts();
    }


    public void DrawCraftStats(Craft craft)
    {
        main.SetActive(true);
        itemName.text = craft.finalItem.item.itemName;

    }

    public void DrawCrafts()
    {
        var crafts = FindObjectOfType<Crafts>().crafts;
        for (int i = 0; i < crafts.Count; i++)
        {
            if (GetComponentInParent<PlayerInventory>().craftTypes.Contains(crafts[i].craftType))
            {
                print("Inst");
                var n = Instantiate(item, holder);
                n.GetComponent<Button>().onClick.AddListener(delegate { DrawCraftStats(crafts[i]); });

                n.SetActive(true);
                n.GetComponentInChildren<TMP_Text>().text = crafts[i].finalItem.item.itemName;
            }
        }
    }
}

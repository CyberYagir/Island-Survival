using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CraftsEditor : EditorWindow
{
    public Vector2 scrollPosition = Vector2.zero;
    public Crafts crafts;
    [MenuItem("YagirLib/Crafts")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CraftsEditor));
    }

    private void OnFocus()
    {
        EditorWindow.GetWindow(typeof(CraftsEditor)).minSize = new Vector2(500, 300);
    }

    private void OnGUI()
    {
        crafts = FindObjectOfType<Crafts>(); 
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        

        GUILayout.BeginVertical();
            for (int i = 0; i < crafts.crafts.Count; i++)
            {
                DrawItem(crafts.crafts[i]);
            }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        if (GUILayout.Button("Add"))
        {
            crafts.crafts.Add(new Craft());
        }
    }

    void GuiLine(int i_height = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    GUIStyle headStyle;
    public void DrawItem(Craft craft)
    {
        headStyle = new GUIStyle();
        headStyle.fontSize = 20;
        headStyle.normal.textColor = Color.white;
        GUILayout.Label(craft.finalItem.item != null ? craft.finalItem.item.itemName : "None", headStyle);
        GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(100));
                craft.craftType = (Craft.CraftType)EditorGUILayout.EnumPopup(craft.craftType);
                craft.finalItem.item = (Item)EditorGUILayout.ObjectField(craft.finalItem.item, typeof(Item), false, GUILayout.Width(100), GUILayout.Height(100));
                if (craft.finalItem != null)
                {
                    craft.finalItem.count = EditorGUILayout.IntField(craft.finalItem.count, GUILayout.Width(100));
                    if (craft.finalItem.count < 1)
                    {
                        craft.finalItem.count = 1;
                    }
                }
                if (GUILayout.Button("Remove"))
                {
                    crafts.crafts.Remove(craft);
                }
        GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(position.size.x - 110));
                for (int i = 0; i < craft.craftItems.Count; i++)
                {
                    DrawCraftItem(craft.craftItems[i], craft);
                }
                if (GUILayout.Button("Add"))
                {
                    craft.craftItems.Add(new CraftItem());
                }
            GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GuiLine();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    public void DrawCraftItem(CraftItem craftItem, Craft craft)
    {
        GUILayout.BeginHorizontal();
        headStyle.fontSize = 12;
        GUILayout.Label(craftItem.item != null ? craftItem.item.itemName : "None", headStyle, GUILayout.Width(100));
        craftItem.item = (Item)EditorGUILayout.ObjectField(craftItem.item, typeof(Item), false, GUILayout.Width(100), GUILayout.Height(20));
        craftItem.count = EditorGUILayout.IntField(craftItem.count, GUILayout.Width(100));
        if (GUILayout.Button("Remove"))
        {
            craft.craftItems.Remove(craftItem);
        }
        GUILayout.EndHorizontal();
    }
}

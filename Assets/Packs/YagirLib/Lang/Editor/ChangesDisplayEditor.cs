using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangesDisplay : EditorWindow
{
    [MenuItem("YagirLib/Changes")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ChangesDisplay));
    }

    private void OnFocus()
    {
        EditorWindow.GetWindow(typeof(ChangesDisplay)).minSize = new Vector2(500, 300);
    }

    private void OnGUI()
    {
        var p = FindObjectOfType<ChangesManager>();
        if (p)
        {
            GUILayout.BeginVertical();
            foreach (var item in p.resChanges)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.TextArea(item.Key.ToString());
                EditorGUILayout.TextArea(item.Value.h.ToString());
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}

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
            int n = 0;
            GUILayout.BeginVertical();
            GUILayout.Label("Changes: ");
            foreach (var item in ChangesManager.changes.changes)
            {
                n++;
                GUILayout.BeginHorizontal();
                EditorGUILayout.TextArea(item.Key.ToString(), GUILayout.Width(200));
                GUILayout.BeginVertical();
                foreach (var vals in item.Value.values)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(vals.Key.ToString() + ": ", GUILayout.Width(50));
                    EditorGUILayout.TextArea(vals.Value.ToString());
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("type: ", GUILayout.Width(50));
                EditorGUILayout.LabelField(item.Value.tp.ToString());
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                if (n > 5) break;
            }
            GUILayout.Space(10);
            GUILayout.Label("Destroys: ");
            foreach (var item in ChangesManager.changes.destroys)
            {
                n++;
                GUILayout.BeginHorizontal();
                EditorGUILayout.TextArea(item.Key.ToString(), GUILayout.Width(200));
                GUILayout.BeginVertical();
                foreach (var vals in item.Value.values)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(vals.Key.ToString() + ": ", GUILayout.Width(50));
                    EditorGUILayout.TextArea(vals.Value.ToString());
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("type: ", GUILayout.Width(50));
                EditorGUILayout.LabelField(item.Value.tp.ToString());
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                if (n > 5) break;
            }
            GUILayout.EndVertical();
        }
    }
}

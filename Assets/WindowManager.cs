using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public List<MoveWindow> moveWindows = new List<MoveWindow>();
    public MoveWindow crafts;
    public static bool menu;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenWindow(crafts);
        }
        menu = false;
        for (int i = 0; i < moveWindows.Count; i++)
        {
            if (moveWindows[i].openClose) { menu = true; break; }
        }
    }

    public void OpenWindow(MoveWindow moveWindow)
    {
        for (int i = 0; i < moveWindows.Count; i++)
        {
            if (moveWindow != moveWindows[i])
            {
                moveWindow.SetOpen(false);
            }
            else
            {
                moveWindow.Toggle();
            }
        }
    }
}

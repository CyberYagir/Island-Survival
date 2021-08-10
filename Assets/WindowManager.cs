using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WindowManager : MonoBehaviour
{
    public List<MoveWindow> moveWindows = new List<MoveWindow>();
    [SerializeField]
    MoveWindow crafts;
    public static bool menu;
    DepthOfField depthOfField;

    private void Start()
    {
        depthOfField = (FindObjectOfType<Volume>().sharedProfile.components[2] as DepthOfField);
    }
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
        depthOfField.focalLength.value += (int)((menu ? 300 : -300) * Time.deltaTime);
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class Keys
{
    public KeyCode keyCode;
    public UnityEvent Down, Up, Press;
}
public class ItemExecuter : MonoBehaviour
{
    public List<Keys> actions;

    private void Update()
    {
        if (!WindowManager.menu)
            for (int i = 0; i < actions.Count; i++)
            {
                if (Input.GetKey(actions[i].keyCode))
                {
                    actions[i].Press.Invoke();
                }
                if (Input.GetKeyDown(actions[i].keyCode))
                {
                    actions[i].Down.Invoke();
                }
                if (Input.GetKeyUp(actions[i].keyCode))
                {
                    actions[i].Up.Invoke();
                }
            }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetQuad : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}

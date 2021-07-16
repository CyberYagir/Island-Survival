using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePhotonConnecting : MonoBehaviour
{
    public Animator animator;
    float time;

    private void OnEnable()
    {
        time = 0;

        animator.Play("ConnectingToPhoton");
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time > 2)
            animator.Play("ConnectingToPhotonOff");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
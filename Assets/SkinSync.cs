using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSync : MonoBehaviour, IPunObservable
{
    public Animator animator;
    public PlayerMove player;

    private void Update()
    {
        animator.SetFloat("RunDir", Mathf.Sign(player.GetForwardSpeed()));
        if (Mathf.Abs(player.GetForwardSpeed()) > 2)
        {
            animator.SetLayerWeight(1, Mathf.Clamp01(animator.GetLayerWeight(1) + Time.deltaTime));
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Clamp01(animator.GetLayerWeight(1) - (Time.deltaTime * 2f)));
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}

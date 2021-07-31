using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour {
    public bool canMine;
    protected ItemsData itemsData;
    protected PlayerInventory playerInventory;
    public AnimationClip attack;

    private void Start()
    {
        canMine = true;
        itemsData = GetComponentInParent<ItemsData>();
        playerInventory = GetComponentInParent<PlayerInventory>();
    }
}



public class MineTool : Usable
{    
    public void Mine()
    {
        if (canMine && !playerInventory.cooldown)
        {
            itemsData.handsAnim.Play(attack.name);
            canMine = false;
            StartCoroutine(startWait());
            StartCoroutine(startAttack());
        }
    }
    IEnumerator startWait()
    {
        yield return new WaitForSeconds(attack.length);
        canMine = true;
    }
    IEnumerator startAttack()
    {
        yield return new WaitForSeconds((playerInventory.GetItem(true) as HandItem).attackTime);
        var res = GetComponentInParent<ItemInteract>().GetResourceFromRay();
        if (res)
        {
            GameManager.manager.LocalPlayer.photonView.RPC("DamageResource", Photon.Pun.RpcTarget.All, res.resId, (int)(playerInventory.GetItem(true) as HandItem).damage);
        }
    }
}
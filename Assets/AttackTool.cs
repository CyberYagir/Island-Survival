using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTool : Usable
{
    public new virtual void Mine()
    {
        if (!(canMine && !playerInventory.cooldown)) return;
        itemsData.handsAnim.Play(attack.name);
        canMine = false;
        StartCoroutine(startWait());
        StartCoroutine(startAttack());
    }
    public new IEnumerator startAttack()
    {
        var live = GetComponentInParent<ItemInteract>().GetHealthFromRay();
        yield return new WaitForSeconds((playerInventory.GetItem(true) as HandItem).attackTime);
        if (live)
        {
            var handI = playerInventory.GetItem(true) as HandItem;
            live.photonView.RPC("TakeDamage", Photon.Pun.RpcTarget.All, (int)handI.damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTool : Usable
{
    public virtual void Mine()
    {
        if (!(canMine && !playerInventory.cooldown)) return;
        itemsData.handsAnim.Play(attack.name);
        canMine = false;
        StartCoroutine(startWait());
        StartCoroutine(startAttack());
    }
    public IEnumerator startAttack()
    {
        yield return new WaitForSeconds((playerInventory.GetItem(true) as HandItem).attackTime);
        var res = GetComponentInParent<ItemInteract>().GetHealthFromRay();
        if (res)
        { 
            //Делать файт (сделать у LiveObject IPunObservable и синхронизировать таким образом хп у всех живих обьектов)
            пукп
            GameManager.manager.LocalPlayer.photonView.RPC("DamageResource", Photon.Pun.RpcTarget.All, res.resId, (res.usableType == GetComponent<ItemExecuter>().type || res.usableType == ItemExecuter.Type.Any ? (int)handI.damage : (int)1));
        }
    }
}

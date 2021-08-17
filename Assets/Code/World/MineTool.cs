using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable {

    public void Start();
    public void Mine();
    public IEnumerator startWait();
    public IEnumerator startAttack();

}

public class Usable : MonoBehaviour, IUsable {
    public bool canMine;
    protected ItemsData itemsData;
    protected PlayerInventory playerInventory;
    public AnimationClip attack;

    public void Start()
    {
        canMine = true;
        itemsData = GetComponentInParent<ItemsData>();
        playerInventory = GetComponentInParent<PlayerInventory>();
    }

    public IEnumerator startWait()
    {
        yield return new WaitForSeconds(attack.length);
        canMine = true;
    }
    public IEnumerator startAttack()
    {
        yield return new WaitForSeconds((playerInventory.GetItem(true) as HandItem).attackTime);
        var res = GetComponentInParent<ItemInteract>().GetResourceFromRay();
        if (res)
        {
            var handI = playerInventory.GetItem(true) as HandItem;
            GameManager.manager.LocalPlayer.photonView.RPC("DamageResource", Photon.Pun.RpcTarget.All, res.resId, (res.usableType == GetComponent<ItemExecuter>().type || res.usableType == ItemExecuter.Type.Any ? (int)handI.damage : (int)1));
        }
    }

    public void Mine()
    {
    }
}



public class MineTool : Usable
{
    public virtual void Mine()
    {
        if (!(canMine && !playerInventory.cooldown)) return;
        itemsData.handsAnim.Play(attack.name);
        canMine = false;
        StartCoroutine(startWait());
        StartCoroutine(startAttack());
    }
    
}

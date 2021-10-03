using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mob : MonoBehaviourPun
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Vector3 newPosition;
    [SerializeField]
    protected float movingRadius;
    [SerializeField]
    public int dropCount;
    public Item dropItem;
    public bool attacked;

    public void SetName()
    {
        transform.name = transform.name.Split('(')[0];
    }

}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mob : LiveObject
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected Vector3 newPosition;
    [SerializeField]
    protected float movingRadius;
    public bool attacked;

}

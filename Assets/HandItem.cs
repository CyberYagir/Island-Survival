using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Hand Item", menuName = "Game/Hand Item", order = 2)]
public class HandItem : Item
{
    public float damage, attackTime;
    public RuntimeAnimatorController animatorController;

}
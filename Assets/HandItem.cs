using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Hand Item", menuName = "Game/Hand Item", order = 2)]
public class HandItem : Item
{
    public float cooldown;
    public float damage;
    public RuntimeAnimatorController animatorController;

}
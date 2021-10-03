using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AnimatorItem: Item
{
    [Newtonsoft.Json.JsonIgnore]
    public RuntimeAnimatorController animatorController;
}

[CreateAssetMenu(fileName = "Hand Item", menuName = "Game/Hand Item", order = 2)]
public class HandItem : AnimatorItem
{
    public float damage, attackTime;

}
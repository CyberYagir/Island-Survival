using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviourPun
{
    public static PlayerStats stats;
    public float health = 100, water = 100, hunger = 100;
    public float t_water = 1, t_hunger = 1;

    public void Awake()
    {
        if (photonView.IsMine)
        {
            stats = this;
        }
    }
    private void Update()
    {

        if (Mathf.Abs(GetComponent<Rigidbody>().velocity.magnitude) > 1)
        {
            water -= t_water * Time.deltaTime;
            hunger -= t_hunger * Time.deltaTime;
        }
    }
}

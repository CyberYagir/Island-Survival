using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviourPun
{
    public static PlayerStats stats;
    public float water = 100, hunger = 100;
    [SerializeField] float t_water = 1, t_hunger = 1;

    public void Awake()
    {
        if (photonView.IsMine)
        {
            stats = this;
        }
    }
    private void Update()
    {
        water = Mathf.Clamp(water, 0, 100);
        hunger = Mathf.Clamp(hunger, 0, 100);
        if (Mathf.Abs(GetComponent<Rigidbody>().velocity.magnitude) > 1)
        {
            water -= t_water * Time.deltaTime;
            hunger -= t_hunger * Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Image hp, water, hungry;
    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }

    private void Update()
    {
        hp.fillAmount = playerStats.health / 100f;
        water.fillAmount = playerStats.water / 100f;
        hungry.fillAmount = playerStats.hunger / 100f;
    }

}

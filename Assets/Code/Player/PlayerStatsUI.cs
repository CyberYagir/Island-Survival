using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] Image hp, water, hungry;
    PlayerStats playerStats;
    LiveObject liveObject;

    private void Start()
    {
           playerStats = GetComponentInParent<PlayerStats>();
        liveObject = playerStats.GetComponent<LiveObject>();
    }

    private void Update()
    {
        hp.fillAmount = liveObject.health / (float)liveObject.maxHealth;
        water.fillAmount = playerStats.water / 100f;
        hungry.fillAmount = playerStats.hunger / 100f;
    }

}

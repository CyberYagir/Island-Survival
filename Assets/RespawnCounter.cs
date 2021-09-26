using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RespawnCounter : MonoBehaviour
{
    [SerializeField] float respawnTime = 5;
    [SerializeField] TMP_Text text;



    private void Update()
    {
        text.text = $"Respawn: <color=\"orange\">{respawnTime.ToString("F0")}</color>";
        respawnTime -= Time.deltaTime;

        if (respawnTime <= 0)
        {
            Destroy(GameManager.manager.LocalPlayer.gameObject);
        }
    }
}

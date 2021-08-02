using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterPoint : MonoBehaviour
{
    public Player player;
    public static WaterPoint waterPoint;

    private void Start()
    {
        waterPoint = this;
    }
    private void Update()
    {
        if (player == null)
        {
            player = FindObjectsOfType<Player>().ToList().Find(x => x.photonView.IsMine);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
    }
}

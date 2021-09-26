using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviourPun
{
    [SerializeField] MonoBehaviour[] behaviours, destroy;
    [SerializeField] GameObject skin, canvas, hands, itemPreview;
    private void Awake()
    {
        GameManager.pause = false;
        foreach (var item in skin.GetComponentsInChildren<Renderer>())
        {
            item.shadowCastingMode = photonView.IsMine ? UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : UnityEngine.Rendering.ShadowCastingMode.On;
        }
        transform.name = photonView.Owner.NickName + ":" + photonView.Owner.ActorNumber;
        if (!photonView.IsMine)
        {
            hands.SetActive(false);
            canvas.SetActive(false);
            itemPreview.SetActive(false);
            GetComponentInChildren<Camera>().enabled = false;
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].enabled = false;
            }
            for (int i = 0; i < destroy.Length; i++)
            {
                Destroy(destroy[i]);
            }
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    [PunRPC]
    public void DamageResource(int resID, int subHp)
    {
        var res = FindObjectOfType<BiomesPrefabsGenerator>().spawnedObjects[resID].GetComponent<Resource>();
        res.hp -= subHp;
        if (res.hp > 0)
        {
            res.Animate();
        }
        if (PhotonNetwork.IsMasterClient)
        {
            ChangesManager.AddChange(resID, new List<Values>(), Change.ChangeType.CreateChange);
            ChangesManager.changes.changes[resID].Set("hp", res.hp);
            if (res.hp <= 0)
            {
                ChangesManager.changes.changes[resID].tp = Change.ChangeType.ResDestroyChange;
            }
        }
        if (res.hp <= 0)
        {
            res.GetComponent<SphereCollider>().enabled = false;
            res.DropResources();
            res.Dead();
            ChangesManager.MoveAllResToDestroy();
        }
        ChangesManager.ReSync(ChangesManager.SyncType.SyncAction);
    }

    public void AddDeath()
    {
        var k = (int)photonView.Owner.CustomProperties["k"];
        var d = (int)photonView.Owner.CustomProperties["d"];
        var newC = new ExitGames.Client.Photon.Hashtable();
        newC.Add("k", k);
        newC.Add("d", d + 1);
        newC.Add("Team", (int)photonView.Owner.CustomProperties["Team"]);
        photonView.Owner.SetCustomProperties(newC);
    }


    public static void RefreshInstance(ref Player player, Player playerPrefab, bool withMasterClient = false)
    {
        if (PhotonNetwork.IsMasterClient == false || withMasterClient == true)
        {
            System.Random rnd = new System.Random(0);

            var pos = new Vector3(rnd.Next(0, 2048) + Random.Range(-10,10),500, rnd.Next(0, 2048) + Random.Range(-10, 10));
            RaycastHit hit;
            Physics.Raycast(pos, Vector3.down, out hit);
            while (!(hit.point.y > 15 && hit.point.y < 45) || Vector3.Angle(hit.normal, Vector3.up) > 10)
            {
                pos = new Vector3(rnd.Next(0, 2048) + Random.Range(-10, 10), 500, rnd.Next(0, 2048) + Random.Range(-10, 10));
                Physics.Raycast(pos, Vector3.down, out hit);
            }
            pos = hit.point + new Vector3(0, 2, 0);

            var rot = Quaternion.identity;
            if (player != null)
            {
                pos = player.transform.position;
                rot = player.transform.rotation;
                PhotonNetwork.Destroy(player.gameObject);
            }
            player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, pos, rot).GetComponent<Player>();
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsSpawner : MonoBehaviour
{
    public List<Mob> mobsPrefabs;
    public List<Mob> spawned_mobs;
    public int maxMobsCount;
    GameObject holder;
    private void Awake()
    {
        holder = new GameObject();
        holder.transform.name = "MobsHolder";
    }

    public void Init()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < maxMobsCount; i++)
            {
                SpawnObject();
            }
        }
        StopAllCoroutines();
        StartCoroutine(cheker());
    }
    IEnumerator cheker()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            spawned_mobs.RemoveAll(x => x == null || x.GetComponent<LiveObject>().health <= 0);
            foreach (var item in spawned_mobs)
            {
                if (item.transform.position.y < 14.5f)
                {
                    var n = item;
                    spawned_mobs.Remove(item);
                    PhotonNetwork.Destroy(n.gameObject);
                    break;
                }
            }
            for (int i = 0; i < maxMobsCount - spawned_mobs.Count; i++)
            {
                SpawnObject();
            }
            
        }
    }

    public void SpawnObject()
    {
        var pos = new Vector3(Random.Range(0, 2000), 40, Random.Range(0, 2000));
        for (int i = 0; i < 20; i++)
        {
            pos = new Vector3(Random.Range(0, 2000), 40, Random.Range(0, 2000));
            if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit))
            {
                if (hit.point.y > 25 && hit.point.y < 38)
                {
                    pos = hit.point + new Vector3(0, 0.5f, 0);
                    break;
                }
            }
        }
        var n = Random.Range(0, mobsPrefabs.Count);
        var obj = PhotonNetwork.Instantiate(mobsPrefabs[n].name, pos, Quaternion.identity);
        obj.transform.name = mobsPrefabs[n].name;
        obj.transform.parent = holder.transform;
        spawned_mobs.Add(obj.GetComponent<Mob>());
    }
}

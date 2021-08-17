using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Linq;


[System.Serializable]
public class ResChange {

    public Dictionary<string, object> values = new Dictionary<string, object>();
    public Change.ChangeType tp = Change.ChangeType.None;
    public ResChange Clone()
    {
        return new ResChange()
        {
            values = values.ToDictionary(entry => entry.Key,
                                               entry => entry.Value),
            tp = tp
        };
    }
    public void Set(string name, object value)
    {
        values.TryGetValue(name, out object get);
        if (get == null)
        {
            values.Add(name, value);
        }
        else
        {
            values[name] = value;
        }
    }
}
public class Values {
    public string name;
    public object value;
}

[System.Serializable] 
public class Change { 
    public enum ChangeType { None, ResChange, CreateChange, ResDestroyChange, DontChange};
    public Dictionary<int, ResChange> changes = new Dictionary<int, ResChange>();
    public Dictionary<int, ResChange> destroys = new Dictionary<int, ResChange>();
    public List<InventorySend> inventorySends = new List<InventorySend>();
}
public class ChangesManager : MonoBehaviour, IPunObservable
{
    public static ChangesManager cm;
    public static Change changes = new Change();
    public enum SyncType { None = 0, SyncAction = 1, SyncPlayer = 2, SyncAll = 3};
    SyncType syncType;
    bool initTerrain;

    public static void ReSync(SyncType syncType)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cm.syncType = syncType;
        }
    }
    public static void AddChange(int id, List<Values> values, Change.ChangeType type = Change.ChangeType.DontChange)
    {
        changes.changes.TryGetValue(id, out ResChange get);
        if (get != null)
        {
            for (int i = 0; i < values.Count; i++)
            {
                get.Set(values[i].name, values[i].value);
            }
        }
        else
        {
            changes.changes.Add(id, new ResChange());
            if (type != Change.ChangeType.DontChange)
            {
                changes.changes[id].tp = type;
            }
            AddChange(id, values, type);
        }
    }

    [PunRPC]
    public void SpawnObject(string itemfilename, Vector3 pos, Quaternion rot)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var n = PhotonNetwork.Instantiate(StaticManager.GetPrefabPathByName(StaticManager.ItemByName(itemfilename).prefab.name), pos, rot); 
            n.GetPhotonView().RPC("Destroys", RpcTarget.AllBuffered);
            if (n.GetComponent<TableRPC>())
            {
                n.GetPhotonView().RPC("SetGlobalResID", RpcTarget.AllBuffered, FindObjectOfType<BiomesPrefabsGenerator>().spawnedObjects.Count);
            }
        }
    }

    public static void MoveAllResToDestroy()
    {
        List<int> keys = new List<int>();
        foreach (var item in changes.changes)
        {
            if (item.Value.tp == Change.ChangeType.ResDestroyChange)
            {
                keys.Add(item.Key);
            }
        }
        for (int i = 0; i < keys.Count; i++)
        {
            changes.destroys.Add(keys[i], changes.changes[keys[i]]);
            changes.changes.Remove(keys[i]);
        }
        if (keys.Count != 0)
        {
            ReSync(SyncType.SyncAll);
        }
    }
    [PunRPC]
    public void CreateDropItem(string name, Vector3 pos, Quaternion quaternion, string prefab, int val, Vector3 forward)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var n = PhotonNetwork.InstantiateRoomObject(name, pos, quaternion);
            n.GetComponent<Rigidbody>().AddForce(forward*2, ForceMode.Impulse);
            if (forward == Vector3.zero)
            {
                n.GetComponent<Drop>().time = n.GetComponent<Drop>().waitTime;
                n.GetComponent<Rigidbody>().AddExplosionForce(500, pos + new Vector3(Random.Range(-5, 5), -1, Random.Range(-5, 5)), 10);
            }
            n.GetPhotonView().RPC("InitRPC", RpcTarget.All, prefab);
            n.GetPhotonView().RPC("SetValue", RpcTarget.All, val);
        }
    }

    public static object GetValue(int id, string paramName)
    {
        if (changes.changes.TryGetValue(id, out ResChange res))
        {
            if (res.values.TryGetValue(paramName, out object val))
            {
                return val;
            }
        }
        return null;
    }

    private void Awake()
    {
        cm = this;
    }
    [PunRPC]
    public void ReSyncRPC()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ReSync(SyncType.SyncAll);            
        }
    }
    [PunRPC]
    public void SendPlayerData(string data)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var dt = JsonConvert.DeserializeObject<InventorySend>(data);
            //print(changes.inventorySends.Count);
            var player = changes.inventorySends.Find(x => x != null && x.playerName == dt.playerName);
            if (player == null)
            {
                changes.inventorySends.Add(dt);
            }
            else
            {
                changes.inventorySends[changes.inventorySends.FindIndex(x => x.playerName == dt.playerName)] = dt;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            int isEq = (int)syncType;
            stream.SendNext(isEq);
            if ((SyncType)isEq == SyncType.SyncAction)
            {
                syncType = SyncType.None;
                stream.SendNext(JsonConvert.SerializeObject(changes.changes));
            }
            else if ((SyncType)isEq == SyncType.SyncPlayer || (SyncType)isEq == SyncType.SyncAll)
            {
                stream.SendNext(JsonConvert.SerializeObject(changes.changes));
                stream.SendNext(JsonConvert.SerializeObject(changes.destroys));
            }
        }
        else
        {
            if (TerrainGenerator.genEnded)
            {
                var type = (SyncType)stream.ReceiveNext();
                if (type == SyncType.SyncAction)
                {
                    changes.changes = JsonConvert.DeserializeObject<Dictionary<int, ResChange>>((string)stream.ReceiveNext());
                }
                else if (type == SyncType.SyncPlayer || type == SyncType.SyncAll)
                {
                    changes.changes = JsonConvert.DeserializeObject<Dictionary<int, ResChange>>((string)stream.ReceiveNext());
                    changes.destroys = JsonConvert.DeserializeObject<Dictionary<int, ResChange>>((string)stream.ReceiveNext());
                }

                if (type == SyncType.SyncAll)
                {
                    if (TerrainGenerator.genEnded && !initTerrain)
                    {
                        FindObjectOfType<BiomesPrefabsGenerator>().SetResources();
                        initTerrain = true;
                    }
                }
            }
        }
    }
}

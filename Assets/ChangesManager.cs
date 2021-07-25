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
    /// <summary>
    /// hp
    /// </summary>
    public int h;


    public ResChange Clone()
    {
        return new ResChange() { h = h };
    }
}

public class ChangesManager : MonoBehaviour, IPunObservable
{
    public static ChangesManager cm;
    public Dictionary<int,ResChange> resChanges = new Dictionary<int, ResChange>();

    public enum SyncType {SyncAction, SyncPlayer, None};
    public SyncType syncType;



    public static void ReSync(SyncType syncType)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cm.syncType = syncType;
        }
    }

    private void Awake()
    {
        cm = this;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            var isEq = syncType != SyncType.None;
            stream.SendNext(isEq);
            if (isEq)
            {
                syncType = SyncType.None;
                stream.SendNext(JsonConvert.SerializeObject(resChanges));
            }
        }
        else
        {
            if ((bool)stream.ReceiveNext() == true)
            {
                var s = (string)stream.ReceiveNext();
                resChanges = JsonConvert.DeserializeObject<Dictionary<int, ResChange>>(s);
            }
        }
    }
}

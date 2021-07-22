using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

[System.Serializable]
public class ResChange {
    public int h;
    public int i;
}

public class ChangesManager : MonoBehaviour, IPunObservable
{
    public List<ResChange> resChanges = new List<ResChange>();
    public List<ResChange> resChangesOld = new List<ResChange>();


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(resChanges != resChangesOld);
            if (resChanges != resChangesOld)
            {
                print("Change");
                stream.SendNext(JsonConvert.SerializeObject(resChanges));
                resChanges = resChangesOld; Пиздец
            }
        }
        if (stream.IsReading)
        {
            if ((bool)stream.ReceiveNext() == true)
            {
                resChanges = JsonConvert.DeserializeObject<List<ResChange>>((string)stream.ReceiveNext());
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OwnerID : NetworkBehaviour {

    [SyncVar]
    public uint ownerID;
}

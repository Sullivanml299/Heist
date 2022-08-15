using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GuardNetworkBehaviour : NetworkBehaviour
{

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!isServer){
            Destroy(GetComponent<MoveTo>());
            Destroy(GetComponent<GuardAnimationHelper>());
        }
    }
}

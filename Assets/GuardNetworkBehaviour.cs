using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GuardNetworkBehaviour : NetworkBehaviour
{
    public MoveTo guard;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!isServer){
            Destroy(GetComponent<MoveTo>());
            Destroy(GetComponent<GuardAnimationHelper>());
        }
    }

    // [Command(requiresAuthority = false)]
    // public void CmdCaught(){
    //     guard.Caught();
    // }
}

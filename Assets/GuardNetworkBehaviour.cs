using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GuardNetworkBehaviour : NetworkBehaviour
{
    public float minAuthorityTransferDistance = 5f;

    NetworkIdentity identity;
    Dictionary<int, NetworkConnectionToClient> connections;
    NetworkConnectionToClient localConnection;
    NetworkConnectionToClient currentAuthority;
    NetworkTransform networkTransform;

    MoveTo guard;

    public void TransferAuthority(NetworkConnectionToClient conn){
        print("Updating Authority!");
        identity.RemoveClientAuthority();
        identity.AssignClientAuthority(conn);
        currentAuthority = conn;

        networkTransform.CancelInvoke();
        networkTransform.Reset();
    }

    void Start(){
        networkTransform = GetComponent<NetworkTransform>();
        identity = GetComponent<NetworkIdentity>();
        connections = NetworkServer.connections;
        localConnection = NetworkServer.localConnection;
        guard = GetComponent<MoveTo>();
        if(isServer) TransferAuthority(localConnection);
    }





    //PUBLIC CALLS
    public void Alert(GameObject newTarget){
        CmdAlert(newTarget.transform);
        // if(guard.currentState != GUARD_STATES.Chasing) CmdAlert(newTarget.transform);
    }
    public void Alert(Transform newTarget){
        CmdAlert(newTarget);
        // if(guard.currentState != GUARD_STATES.Chasing) CmdAlert(newTarget);
    }

    public void Caught(){
        CmdCaught();
    }



    //COMMANDS
    [Command(requiresAuthority = false)]
    public void CmdCaught(){
        guard.Caught();
    }



    [Command (requiresAuthority = false)]
    void CmdAlert(Transform newTarget){
        if(CanBeAlerted(newTarget)) guard.Alert(newTarget);
    }



    //UTILITIES
    bool CanBeAlerted(Transform newTarget){
        if( guard.currentState != GUARD_STATES.Chasing ||
            guard.currentTarget == newTarget.gameObject) return true;
        return false;
    }


    // public void TransferAuthority(int connID){
    //     print("Updating Authority!");
    //     var conn = connections[connID]; 
    //     identity.RemoveClientAuthority();
    //     identity.AssignClientAuthority(conn);
    //     currentAuthority = conn;

    //     networkTransform.CancelInvoke();
    //     networkTransform.Reset();
    // }

    // public override void OnStartAuthority()
    // {
    //     base.OnStartAuthority();
    //     networkTransform.CancelInvoke();
    //     networkTransform.Reset();
    // }

    // public override void OnStopAuthority()
    // {
    //     base.OnStopAuthority();
    //     networkTransform.CancelInvoke();
    //     networkTransform.Reset();
    // }


    // void Update(){
    //     // if(isServer) updateControl();
    // }

    // void updateControl(){
    //     if(!guard.isChasing()) return; //Don't update state if the guard is chasing
    //     NetworkConnectionToClient newAuthority = guard.getTarget();
    //     if(newAuthority != null && newAuthority != currentAuthority) TransferAuthority(newAuthority);
    // }


}

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

    public void TransferAuthority(int connID){
        print("Updating Authority!");
        var conn = connections[connID]; 
        identity.RemoveClientAuthority();
        identity.AssignClientAuthority(conn);
        currentAuthority = conn;
        networkTransform.CancelInvoke();
        networkTransform.Reset();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        networkTransform.CancelInvoke();
        networkTransform.Reset();
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();
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

    void Update(){
        if(isServer) updateControl();
    }

    // public override void OnStartClient()
    // {
    //     base.OnStartClient();
    //     // if(!isServer){
    //     //     // GetComponent<MoveTo>().enabled = false;
    //     //     // GetComponent<GuardAnimationHelper>().enabled = false;
    //     //     // this.enabled = false;
    //     //     // Destroy(GetComponent<MoveTo>());
    //     //     // Destroy(GetComponent<GuardAnimationHelper>());
    //     // }
    // }

    void updateControl(){
        if(!guard.isPatrolling()) return; //Don't update state if the guard is chasing
        // print("Update");
        // print(connections.Count);
        float minDistance = Mathf.Infinity;
        float _distance = Mathf.Infinity;
        NetworkConnectionToClient newAuthority= null;
        if(isServer){
            foreach(NetworkConnectionToClient client in connections.Values){
                _distance = getDistanceToClient(client);
                // print(_distance);
                if(_distance<minDistance && Mathf.Abs(_distance - minDistance)>minAuthorityTransferDistance){
                    newAuthority = client;
                    minDistance = _distance;
                }
            }
        }

        // print(newAuthority);
        if(newAuthority != null && newAuthority != currentAuthority) TransferAuthority(newAuthority);
    }

    //TODO: could make this faster by storing a list of player objects
    float getDistanceToClient(NetworkConnectionToClient client){ 
        foreach (NetworkIdentity obj in client.clientOwnedObjects){
            // print(obj);
            if(obj.gameObject !=  gameObject && obj.gameObject.tag == "Player"){
                return Vector3.Distance(obj.transform.position, transform.position);
            }
        }
        return Mathf.Infinity;
    }

    // [Command(requiresAuthority = false)]
    // public void CmdCaught(){
    //     guard.Caught();
    // }
}

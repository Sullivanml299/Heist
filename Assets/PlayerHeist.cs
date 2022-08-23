using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHeist : NetworkBehaviour
{
    public CharacterController playerController;
    public Transform guard;
    public GuardNetworkBehaviour guardController;
    Vector3 startPosition;
    bool returnHome = false;
    void Start(){
        startPosition = transform.position;
        guard = GameObject.Find("Guard (1)").transform;
        guardController = GameObject.Find("Guard (1)").GetComponent<GuardNetworkBehaviour>();
        playerController = GetComponent<CharacterController>();
    }

    void LateUpdate(){
        if(returnHome){
            transform.position = startPosition;
            returnHome = false;
            playerController.enabled = true;
        }
    }

    public Vector3 getStart(){
        return startPosition;
    } 

    [TargetRpc]
    public void TargetReturnToStart(){
        playerController.enabled = false;
        returnHome = true;
        // transform.position = startPosition;
        Debug.Log("RPC");
    }

    public void LocalReturnToStart(){
        if(hasAuthority) {
            playerController.enabled = false;
            transform.position = startPosition;
            playerController.enabled = true;
            Debug.Log("LOCAL");
        }
        else TargetReturnToStart();
    }

    public NetworkConnectionToClient getConnection(){
        return this.connectionToClient;
    }

}

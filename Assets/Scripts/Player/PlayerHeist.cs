using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using StarterAssets;

public class PlayerHeist : NetworkBehaviour
{
    public CharacterController playerController;
    public ThirdPersonController thirdPersonController;
    public Transform guard;
    public GuardNetworkBehaviour guardController;
    public NetworkTransform localTransform;
    PlayerSkills playerSkills;
    Vector3 startPosition;
    bool returnHome = false;


    void Start(){
        startPosition = transform.position;
        guard = GameObject.Find("Guard (1)").transform;
        guardController = GameObject.Find("Guard (1)").GetComponent<GuardNetworkBehaviour>();
        playerController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        playerSkills = GetComponent<PlayerSkills>();
        localTransform = GetComponent<NetworkTransform>();
    }

    void LateUpdate(){
        if(returnHome && Vector3.Distance(transform.position, startPosition) <0.1f){
            print("Return Home");
            // transform.position = startPosition;
            returnHome = false;
            playerController.enabled = true;
            thirdPersonController.enabled = true;
        }
    }

    public Vector3 getStart(){
        return startPosition;
    } 

    [TargetRpc]
    public void TargetReturnToStart(){
        playerController.enabled = false;
        thirdPersonController.enabled = false;
        returnHome = true;
        localTransform.CmdTeleport(startPosition);
        transform.position = startPosition;
        Debug.Log("RPC");
    }

    public void LocalReturnToStart(){
        if(hasAuthority) {
            playerController.enabled = false;
            transform.position = startPosition;
            playerController.enabled = true;
            // Debug.Log("LOCAL");
        }
        else TargetReturnToStart();
    }

    public NetworkConnectionToClient getConnection(){
        return this.connectionToClient;
    }


}

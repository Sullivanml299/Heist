
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public enum GUARD_STATES{
    Patrolling,
    Chasing,
    Investigating,
}

public class MoveTo : NetworkBehaviour {

    public Transform[] patrolRoute;

    [SyncVar]
    public Transform targetDestination;
    public GameObject currentTarget;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float chaseCooldownMax = 0.5f;
    public float currentChaseCooldown = 0f;

    NavMeshAgent agent;

    [SyncVar]
    int patrolIndex = 0;
    [SyncVar(hook = nameof(stateChange))]
    public GUARD_STATES currentState = GUARD_STATES.Patrolling;
   

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        SetTarget(patrolRoute[patrolIndex]);//targetDestination = patrolRoute[patrolIndex];
        setSpeed(walkSpeed);
    }

    void FixedUpdate(){
        if(hasAuthority) move();
    }


    public void Alert(Transform target){
        
        if(currentState == GUARD_STATES.Chasing){
            SetTarget(target);
        }
        else if(currentState == GUARD_STATES.Patrolling && currentChaseCooldown <= 0){
            setState(GUARD_STATES.Chasing);
            SetTarget(target.transform);// targetDestination = target.transform;
            currentChaseCooldown = chaseCooldownMax;
        }
    }

    public void Caught(){
        if(hasAuthority) setState(GUARD_STATES.Patrolling);
        currentChaseCooldown = chaseCooldownMax;
    }

    void stateChange(GUARD_STATES oldState, GUARD_STATES newState){
        print("StateChange " + newState);
        switch(newState){
            case GUARD_STATES.Patrolling:
                setSpeed(walkSpeed);
                break;
            
            case GUARD_STATES.Chasing:
                setSpeed(runSpeed);
                break;
        }
    }

    // public override void OnStartAuthority()
    // {
    //     base.OnStartAuthority();
    //     print(currentState);
    //     switch(currentState){
    //         case GUARD_STATES.Patrolling:
    //             setSpeed(walkSpeed);
    //             break;
            
    //         case GUARD_STATES.Chasing:
    //             setSpeed(runSpeed);
    //             break;
    //     }
    // }

    // public override void OnStopAuthority()
    // {
    //     base.OnStopAuthority();
    // }



    void move(){
        if(targetDestination == null)SetTarget(patrolRoute[patrolIndex]);// targetDestination = patrolRoute[patrolIndex];
        // print("CurrentState: " + currentState);
        // print("target Destination:" + targetDestination);
        // print("patrolIndex: " + patrolIndex);
        if(currentChaseCooldown > 0){
            currentChaseCooldown -= Time.deltaTime;
        }
        // print(targetDestination.gameObject);
        switch(currentState){
            case GUARD_STATES.Patrolling:
                Patrol();
                break;
            
            case GUARD_STATES.Chasing:
                //chase to the last known location
                Chase();
                break;
            
            case GUARD_STATES.Investigating:
                //if you reach the last known position look around in points of interest
                break;
        }
        agent.destination = targetDestination.transform.position;
    }

    //TODO: add state transition logic

    void Patrol(){
        if(Vector3.Distance(targetDestination.transform.position, transform.position) < 2f) {
            //TODO: add in a short delay and maybe a back and forth look 
            CmdUpdatePatrolIndex();
            SetTarget(patrolRoute[patrolIndex]);//targetDestination = patrolRoute[patrolIndex];
        }
    }

    void Chase(){
        // if(Vector3.Distance(targetDestination.transform.position, transform.position) < 2f) {
        //     GameObject target = targetDestination.gameObject;
        //     // var returnPosition = targetDestination.gameObject.GetComponent<PlayerHeist>().getStart();
        //     // targetDestination.gameObject.transform.position = returnPosition;
        //     Debug.Log("SEND RPC");
        //     target.GetComponent<PlayerHeist>().LocalReturnToStart();
        //     // if(isServer) targetDestination.gameObject.GetComponent<PlayerHeist>().LocalReturnToStart();
        //     // else targetDestination.gameObject.GetComponent<PlayerHeist>().TargetReturnToStart();
        //     setState(GUARD_STATES.Patrolling);
        // }
    }

    void UpdatePatrolPoint(){
        patrolIndex = (patrolIndex < patrolRoute.Length-1) ? (patrolIndex + 1) : 0;
    }

    void setSpeed(float speed){
        agent.speed = speed;
    }

    void targetClosestPatrolPoint(){
        float minDistance = 1000;
        Transform target = null;
        Transform point;
        float distance;

        for(int i = 0; i < patrolRoute.Length; i++){
            point = patrolRoute[i].transform;
            distance = Vector3.Distance(point.position, transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                target = point;
                patrolIndex = i;
            }
        }

        SetTarget(target);// targetDestination = target;
    }

    public bool isPatrolling(){
        return currentState == GUARD_STATES.Patrolling;
    }

    public bool isChasing(){
        return currentState == GUARD_STATES.Chasing;
    }

    public NetworkConnectionToClient getTarget(){ //TODO: fix this. this is terrible
        return targetDestination.gameObject.GetComponent<PlayerHeist>().getConnection();
    }

    public float DistanceFromTarget(){
        return Vector3.Distance(transform.position, targetDestination.position);
    }

    void setState(GUARD_STATES newState){
        switch(currentState){
            case GUARD_STATES.Patrolling:
                break;

            case GUARD_STATES.Chasing:
                break;

            case GUARD_STATES.Investigating:

                break;
        }

        switch(newState){
            case GUARD_STATES.Patrolling:
                setSpeed(walkSpeed);
                targetClosestPatrolPoint();
                break;

            case GUARD_STATES.Chasing:
                setSpeed(runSpeed);
                break;

            case GUARD_STATES.Investigating:
                break;
        }

        currentState = newState;
        CmdSetState(newState);
    }


    void SetTarget(Transform newTarget){
        // targetDestination = newTarget;
        currentTarget = newTarget.gameObject;
        targetDestination.position = newTarget.position;
        CmdSetTarget(newTarget);
    }

    [Command]
    void CmdUpdatePatrolIndex(){
        patrolIndex = (patrolIndex < patrolRoute.Length-1) ? (patrolIndex + 1) : 0;
    }

    [Command]
    void CmdSetState(GUARD_STATES newState){
        currentState = newState;
    }

    [Command]
    void CmdSetTarget(Transform newTarget){
        // targetDestination = newTarget;
        targetDestination.position = newTarget.position;
    }




}
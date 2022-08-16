
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
    public Transform targetDestination;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float chaseCooldownMax = 0.5f;

    public float currentChaseCooldown = 0f;

    NavMeshAgent agent;
    int patrolIndex = 0;
    GUARD_STATES currentState = GUARD_STATES.Patrolling;
   

    public void Alert(GameObject target){
        
        if(currentState == GUARD_STATES.Patrolling && currentChaseCooldown <= 0){
            setState(GUARD_STATES.Chasing);
            targetDestination = target.transform;
            currentChaseCooldown = chaseCooldownMax;
        }
    }

    public void Caught(){
        setState(GUARD_STATES.Patrolling);
    }

    
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        targetDestination = patrolRoute[patrolIndex];
        setSpeed(walkSpeed);
    }

    void FixedUpdate(){
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
        agent.destination = targetDestination.position;
    }

    //TODO: add state transition logic

    void Patrol(){
        if(Vector3.Distance(targetDestination.position, transform.position) < 2f) {
            //TODO: add in a short delay and maybe a back and forth look 
            UpdatePatrolPoint();
            targetDestination =  patrolRoute[patrolIndex];
        }
    }

    void Chase(){
        if(Vector3.Distance(targetDestination.position, transform.position) < 2f) {
            GameObject target = targetDestination.gameObject;
            // var returnPosition = targetDestination.gameObject.GetComponent<PlayerHeist>().getStart();
            // targetDestination.gameObject.transform.position = returnPosition;
            Debug.Log("SEND RPC");
            target.GetComponent<PlayerHeist>().LocalReturnToStart();
            // if(isServer) targetDestination.gameObject.GetComponent<PlayerHeist>().LocalReturnToStart();
            // else targetDestination.gameObject.GetComponent<PlayerHeist>().TargetReturnToStart();
            setState(GUARD_STATES.Patrolling);
        }
    }

    void UpdatePatrolPoint(){
        patrolIndex = (patrolIndex < patrolRoute.Length-1) ? (patrolIndex + 1) : 0;
    }

    void setSpeed(float speed){
        agent.speed = speed;
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
    }

    void targetClosestPatrolPoint(){
        float minDistance = 1000;
        Transform target = null;
        Transform point;
        float distance;

        for(int i = 0; i < patrolRoute.Length; i++){
            point = patrolRoute[i];
            distance = Vector3.Distance(point.position, transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                target = point;
                patrolIndex = i;
            }
        }

        targetDestination = target;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    

public enum GUARD_STATES{
    Patrolling,
    Chasing,
    Investigating,
}

public class MoveTo : MonoBehaviour {

    public Transform[] patrolRoute;
    public Transform targetDestination;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;

    NavMeshAgent agent;
    int patrolIndex = 0;
    GUARD_STATES currentState = GUARD_STATES.Patrolling;
    
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        targetDestination = patrolRoute[patrolIndex];
        setSpeed(walkSpeed);
    }

    void FixedUpdate(){
        switch(currentState){
            case GUARD_STATES.Patrolling:
                Patrol();
                break;
            
            case GUARD_STATES.Chasing:
                //chase to the last known location
                break;
            
            case GUARD_STATES.Investigating:
                //if you reach the last known position look around in points of interest
                break;
        }
    }

    //TODO: add state transition logic

    void Patrol(){
        if(Vector3.Distance(targetDestination.position, transform.position) > 2f) {
           agent.destination = targetDestination.position;
        }
        else{
            //TODO: add in a short delay and maybe a back and forth look 
            UpdatePatrolPoint();
            targetDestination =  patrolRoute[patrolIndex];
        }
    }

    void UpdatePatrolPoint(){
        patrolIndex = (patrolIndex < patrolRoute.Length-1) ? (patrolIndex + 1) : 0;
    }

    void setSpeed(float speed){
        agent.speed = speed;
    }

}
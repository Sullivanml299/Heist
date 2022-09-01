using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    private GameObject lastGuard;
    private GuardNetworkBehaviour lastGuardNetworker;
    private PlayerHeist thisPlayer;

    // Start is called before the first frame update
    void Start()
    {
        thisPlayer = transform.parent.GetComponent<PlayerHeist>();
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    private void OnTriggerEnter(Collider collider){
        // if(collider.tag == "Guard" && collider.gameObject != lastGuard){
        //     print("TRIGGER!");
        //     lastGuard = collider.gameObject;
        //     lastGuardNetworker = collider.GetComponent<GuardNetworkBehaviour>();
        //     lastGuardNetworker.Caught();
        //     thisPlayer.LocalReturnToStart();
        // }
        // print(collider.tag);
        if(collider.tag == "Guard"){
            print("Caught");
            collider.GetComponent<MoveTo>().Caught();
            thisPlayer.LocalReturnToStart();
        }
    }

}

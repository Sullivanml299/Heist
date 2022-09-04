using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameController gameController;
    void OnTriggerEnter(Collider collider){
        if( collider.gameObject.tag == "Player" || collider.gameObject.tag == "StealthPlayer")
        {
            PlayerInventory playerInventory;
            var localPlayer = collider.gameObject.TryGetComponent<PlayerInventory>(out playerInventory);
            var objective = gameController.gameObjective();
            if(localPlayer && playerInventory.hasItem(objective)) gameController.callEndGame();
        }
    }
}

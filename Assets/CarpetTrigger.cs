using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetTrigger : MonoBehaviour
{
    public string switchGroup;
    public string enterState = "Carpet";
    public string exitState = "Wood";

    void OnTriggerEnter(Collider collider){
        // print(collider.gameObject.tag);
        if( collider.gameObject.tag == "Guard" 
            || collider.gameObject.tag == "Player"
            || collider.gameObject.tag == "StealthPlayer") AkSoundEngine.SetSwitch(switchGroup, enterState, collider.gameObject);
    }

    void OnTriggerExit(Collider collider){
        // print(collider.gameObject.tag);
        if( collider.gameObject.tag == "Guard" 
            || collider.gameObject.tag == "Player"
            || collider.gameObject.tag == "StealthPlayer") AkSoundEngine.SetSwitch(switchGroup, exitState, collider.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AK.Wwise.Event musicEvent;
    public bool hostOnly = true;
    public string switchGroup;
    public string chaseState = "Chase";
    public string normalState = "Normal";

    public void startMusic(bool isHost){
        if( !hostOnly || (hostOnly && isHost)) musicEvent.Post(gameObject);
    }

    public void stopMusic(bool isHost){
        if( !hostOnly || (hostOnly && isHost)) musicEvent.Stop(gameObject);
    }

    public void setChaseState(bool value){
        print("SET MUSIC STATE " + value);
        if(value) AkSoundEngine.SetSwitch(switchGroup, chaseState, gameObject);
        else AkSoundEngine.SetSwitch(switchGroup, normalState, gameObject);
    }

}

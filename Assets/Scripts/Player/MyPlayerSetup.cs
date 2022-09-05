using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif
using StarterAssets;


public class MyPlayerSetup : NetworkBehaviour
{
    public CinemachineVirtualCamera _camera;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _camera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        _camera.Follow = gameObject.transform.GetChild(0);

        GetComponent<PlayerInput>().enabled = true;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<ThirdPersonController>().enabled =  true;
        GetComponent<PlayerInventory>().enabled =  true;

        GameObject.Find("MusicController").GetComponent<MusicController>().startMusic(isServer);

    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        GameObject.Find("MusicController").GetComponent<MusicController>().stopMusic(isServer);
    }

    //This is called anytime a networked object is spawned on a connected client.
    //this includes the player's own object, but we can use this to apply actions only to 
    //non-player objects
    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!isLocalPlayer){
            Destroy(GetComponent<ThirdPersonController>());
            Destroy(GetComponent<PlayerInput>());
            // Destroy(GetComponent<CharacterController>());
            // Destroy(GetComponent<PlayerInventory>());
        }
    }
}

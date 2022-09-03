using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Mirror;

public class PlayerSkills : NetworkBehaviour
{
    public SkinnedMeshRenderer _renderer;
    public Material stealthMaterial;
    public Material[] normalMaterials;
    public Material[] stealthMaterials;
    public bool useTrail = true;
    public GameObject trail;
    StarterAssetsInputs _input;
    ThirdPersonController _playerController;
    PlayerHeist playerHeist;
    // private GuardsController guardsController;

    [SyncVar(hook = nameof(syncStealthState))]
    bool stealthActive = false;
    Animator _animator;
    private int _animIDSneak;
    private int stealthLayer;
    private int normalLayer;


    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        _animIDSneak = Animator.StringToHash("Sneak");
        _playerController = GetComponent<ThirdPersonController>();
        playerHeist = GetComponent<PlayerHeist>();
        // guardsController = GameObject.Find("GuardsController").GetComponent<GuardsController>();

        stealthLayer = LayerMask.NameToLayer("StealthLayer");
        normalLayer = gameObject.layer;

        normalMaterials = _renderer.materials;
        stealthMaterials = new Material[] {stealthMaterial, stealthMaterial, stealthMaterial};

    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer) updateStealthState();

    }

    void toggleStealth(){
        stealthActive = !stealthActive;
        if(stealthActive) activateStealthMaterials();
        else deactivateStealthMaterials();
    }

    public void setStealth(bool value){
        print("SetStealth " + value);
        stealthActive = value;
        if(value) activateStealthMaterials();
        else deactivateStealthMaterials();
    }

    void activateStealthMaterials(){
        // print("Activate");
        _renderer.materials= stealthMaterials;
        _renderer.shadowCastingMode= UnityEngine.Rendering.ShadowCastingMode.Off;
        trail.SetActive(useTrail);
        _animator.SetBool(_animIDSneak, true);
        _playerController.Sneaking = true;
        // this.gameObject.tag = "StealthPlayer";
        this.gameObject.layer = stealthLayer;
        if(!isServer) CmdSetStealth(true);
        // guardsController.setPlayerStealth(true);
    }

    void deactivateStealthMaterials(){
        // print("DEActivate");
        _renderer.materials= normalMaterials;
        _renderer.shadowCastingMode= UnityEngine.Rendering.ShadowCastingMode.On;
        trail.SetActive(false);
        _animator.SetBool(_animIDSneak, false);
        _playerController.Sneaking = false;
        // this.gameObject.tag = "Player";
        this.gameObject.layer = normalLayer;
        if(!isServer) CmdSetStealth(false);
        // guardsController.setPlayerStealth(false); // turn off guard detection;
    }


    void updateStealthState(){
        if(_input.jump){
            // print("TOGGLE");
            toggleStealth();
            _input.jump = false;
        }
        if(stealthActive && (_input.sprint || _input.loot)) setStealth(false);
    }

    [Command] //For clients telling the server
    void CmdSetStealth(bool val){
        setStealth(val);
    }

    //for server telling clients
    void syncStealthState(bool oldState, bool newState){
        if(!isLocalPlayer) setStealth(newState);
    }
}

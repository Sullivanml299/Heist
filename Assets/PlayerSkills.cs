using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Mirror;

public class PlayerSkills : MonoBehaviour
{
    public SkinnedMeshRenderer _renderer;
    public Material stealthMaterial;
    public Material[] normalMaterials;
    public Material[] stealthMaterials;
    public bool useTrail = true;
    public GameObject trail;
    StarterAssetsInputs _input;
    bool stealthActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!GetComponent<NetworkIdentity>().isLocalPlayer) {
            this.enabled = false;
            return;
        }
        normalMaterials = _renderer.materials;
        stealthMaterials = new Material[] {stealthMaterial, stealthMaterial, stealthMaterial};
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_input.jump){
            // print("TOGGLE");
            toggleStealth();
            _input.jump = false;
        }
    }

    void toggleStealth(){
        stealthActive = !stealthActive;
        if(stealthActive) activateStealthMaterials();
        else deactivateStealthMaterials();
    }

    void activateStealthMaterials(){
        // print("Activate");
        _renderer.materials= stealthMaterials;
        _renderer.shadowCastingMode= UnityEngine.Rendering.ShadowCastingMode.Off;
        trail.SetActive(useTrail);
    }

    void deactivateStealthMaterials(){
        // print("DEActivate");
        _renderer.materials= normalMaterials;
        _renderer.shadowCastingMode= UnityEngine.Rendering.ShadowCastingMode.On;
        trail.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerInventory : MonoBehaviour
{
    public List<Loot> inventory = new List<Loot>();
    public float minLootDistance = 1f;
    public float raycastYOffset = 1.1f;
    public float raycastXOffset = -1f;
    public float spherecastRadius = 0.5f;
    public LayerMask mask;

    private StarterAssetsInputs _input;
    private GameObject raycastObject;
    private Container container;
    private Animator _animator;
    private ThirdPersonController _controller;
    private int _animIDLootLow;
    private int _animIDLootHigh;
    

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<ThirdPersonController>();

        _animIDLootLow = Animator.StringToHash("LootingLow");
        _animIDLootHigh = Animator.StringToHash("LootingHigh");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var origin = transform.position + Vector3.up * raycastYOffset + transform.forward * raycastXOffset;
        
        if(Physics.SphereCast( origin, spherecastRadius, 
                            transform.forward, out hit, minLootDistance, mask)){

            if( raycastObject == null || raycastObject != hit.collider.gameObject) {
                registerContainer(hit);
            }
        }else if(raycastObject != null) {
                unregisterContainer();
        }



        Debug.DrawRay(transform.position + Vector3.up * raycastYOffset, transform.forward * minLootDistance, Color.blue);
        // print(_input.loot);
        if(_input.loot) {
            print("Looting");
            // _animator.SetBool(_animIDLootHigh, true);
            lootRegisteredContainer();
        }
        // else{
        //     // _animator.SetBool(_animIDLootHigh, false);
        // }
    }

    void lootRegisteredContainer(){
        if(container == null || !container.hasLoot()) return;
        container.lootAll(inventory);
        if(!container.hasLoot()) unregisterContainer();
    }

    void registerContainer(RaycastHit hit){
        raycastObject = hit.collider.gameObject;
        container = raycastObject.GetComponent<Container>();
        if(container.hasLoot()) container.setFocus(true);
        else container.setFocusEmpty(true);
    }

    void unregisterContainer(){
        container.setFocus(false);
        raycastObject = null;
        container = null;
    }
}

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
    private CharacterController _moveController;
    private int _animIDLootLow;
    private int _animIDLootHigh;
    private UIController mainUI;
    

    // Start is called before the first frame update
    void Start()
    {
        if(!GetComponent<PlayerHeist>().isLocalPlayer) {
            this.enabled = false;
            return;
        }
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<ThirdPersonController>();
        _moveController = GetComponent<CharacterController>();

        _animIDLootLow = Animator.StringToHash("LootingLow");
        _animIDLootHigh = Animator.StringToHash("LootingHigh");

        mainUI = GameObject.Find("MainUI").GetComponent<UIController>();

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
        // print("INPUT " + _input.loot);
        if(_input.loot && container != null) {
            // print("Looting");
            _animator.SetBool(_animIDLootLow, true);
            _moveController.enabled = false;
            _controller.enabled = false;
            // lootRegisteredContainer();
        }
        else{
            _animator.SetBool(_animIDLootLow, false);
            _moveController.enabled = true;
            _controller.enabled = true;
        }
    }

    public void addItem(Loot loot){
        print("Add ITEM");
        inventory.Add(loot);
        mainUI.newNotification(loot);
        //TODO: add some message here;

    }

    public void containerWasEmpty(){
        print("container is empty");
    }

    public bool hasItem(Loot item){
        return inventory.Contains(item);
    }

    public void unregisterContainer(Container oldContainer = null){
        print("Unregister " +( oldContainer != null && oldContainer != container));
        if(oldContainer != null && oldContainer != container) return;
        container.setFocus(false);
        raycastObject = null;
        container = null;
    }

    void registerContainer(RaycastHit hit){
        raycastObject = hit.collider.gameObject;
        container = raycastObject.GetComponent<Container>();
        if(container.hasLoot()) container.setFocus(true);
        else container.setFocusEmpty(true);
    }


    void lootRegisteredContainer(){
        if(container == null || !container.hasLoot()) return;
        container.loot(this);
        // container.lootAll(inventory);
        if(!container.hasLoot()) unregisterContainer(); //TODO: need to update has loot to check with the server somehow
    }




    
}

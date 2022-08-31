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
    

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var origin = transform.position + Vector3.up * raycastYOffset + transform.forward * raycastXOffset;
        
        if(Physics.SphereCast( origin, spherecastRadius, 
                            transform.forward, out hit, minLootDistance, mask)){

            if( raycastObject == null || raycastObject != hit.collider.gameObject) {
                raycastObject = hit.collider.gameObject;
                container = raycastObject.GetComponent<Container>();
                container.setFocus(true);
            }
        }else if(raycastObject != null) {
            container.setFocus(false);
            raycastObject = null;
            container = null;
        }



        Debug.DrawRay(transform.position + Vector3.up * raycastYOffset, transform.forward * minLootDistance, Color.blue);
        // print(_input.loot);
        if(_input.loot) {
            print("Looting");
        }
    }
}

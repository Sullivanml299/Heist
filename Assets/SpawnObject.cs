using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnObject : NetworkBehaviour
{

    public GameObject myObjectPrefab;
    private GameObject myObject; //this is a server side variable. Will never be filled on the client
    private bool isSpawned = false;
    
    [SyncVar(hook = nameof(CounterChange))]
    private int counter = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && hasAuthority){
            if (!isSpawned){
                CmdSpawnObject();
                isSpawned = true;
            }
            else {
                CmdDestroyObject();
                isSpawned = false;
            }
        }

    }

    void CounterChange(int oldCounter, int newCounter){
        Debug.Log("Counter changed from " + oldCounter + " to " + newCounter);
    }

    [Command]
    void CmdSpawnObject(){
        myObject = Instantiate(myObjectPrefab);
        NetworkServer.Spawn(myObject, connectionToClient);
        counter++;
    }

    [Command]
    void CmdDestroyObject(){
        NetworkServer.Destroy(myObject);
        myObject = null;
    }
}

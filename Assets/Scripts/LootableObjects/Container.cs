using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Outline), typeof(NetworkIdentity))]
public class Container : NetworkBehaviour
{
    public ContainerSettings settings;
    // public int minItemCount = 1;
    // public int maxItemCount = 5;
    public List<Loot> contents = new List<Loot>();
    public Outline outline;
    [SyncVar(hook = nameof(countSynced))]
    public int itemCount;

    [SyncVar(hook = nameof(seedSynced))]
    private int seed;
    private PlayerInventory localInventory;



    // Start is called before the first frame update
    void Start()
    {
        if(outline == null) {
            outline = GetComponent<Outline>();
            setFocus(false);
        }
        if(isServer) serverSetUp();
    }


    public void setFocus(bool value){
        outline.enabled = value;
    }

    public void setFocusEmpty(bool value){
        outline.enabled = value;
        outline.OutlineColor = Color.yellow;
    }

    public bool hasLoot(){
        if(itemCount > 0) return true;
        return false;
    }

    public void loot(PlayerInventory playerInventory){
        localInventory = playerInventory;
        CmdLootItem();
    }

    public void lootAll(List<Loot> targetInventory){
        foreach(Loot loot in contents){
            targetInventory.Add(loot);
        }
        contents.Clear();
    }

    void serverSetUp(){
        seed = Random.Range(0,1000000);
        fillContainer(seed);
        // printContents();
    }

    void seedSynced(int oldSeed, int newSeed){
        // print("SyncSeed!");
        if(!isServer) fillContainer(newSeed);
    }

    void countSynced(int oldCount, int newCount){
        // print("SyncCount");
        if(!isServer && newCount<=0 && localInventory != null) localInventory.unregisterContainer(this); 
    }

    void fillContainer(int seed = 0){
        // print("FILLING");
        Random.InitState(seed);
        itemCount = Random.Range(settings.minItemCount, settings.maxItemCount+1); //add 1 because int version is exclusive on high end
        // print("COUNT; " + itemCount);
        float roll;
        float currentProbability;

        for(var i = 0; i<itemCount; i++){
            roll = Random.value;
            // print("roll:" + roll);
            currentProbability = 0;
            foreach(LootSettings itemSettings in settings.possibleLoot){
                currentProbability += itemSettings.spawnRate;
                if(roll <= currentProbability) {
                    // print("adding");
                    contents.Add(itemSettings.loot);
                    break;
                }
            }
        }
    }

    void printContents(){
        foreach(Loot loot in contents){
            print(loot.name + " " + loot.value);
        }
    }


    //COMMANDS (client calls, server runs)
    [Command(requiresAuthority = false)]
    void CmdLootItem(NetworkConnectionToClient sender = null){
        //Since I'm using scriptable objects to represent loot, I can't send those easily using mirror.
        //Instead I deterministically fill the container with items locally and then tell the client the index to loot.
        //This mechanism is the best simple way to ensure that looting doesn't get weird with the networking
        //Give items in order in the contents list, first to last.

        if(itemCount > 0) { 
            int itemIndex = contents.Count - itemCount;
            RpcGivePlayerLoot(sender, itemIndex);
            itemCount--;
        }
        else RpcGivePlayerLoot(sender, -1); //-1 means out of items
    }

    //RPC CALLS (server calls, client runs)
    [TargetRpc]
    public void RpcGivePlayerLoot(NetworkConnection target, int itemIndex){
        if(itemIndex == -1){
            localInventory.containerWasEmpty();
            return;
        }
        localInventory.addItem(contents[itemIndex]);
    }

}

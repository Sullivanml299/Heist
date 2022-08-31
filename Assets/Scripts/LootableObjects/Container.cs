using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public ContainerSettings settings;
    public int minItemCount = 1;
    public int maxItemCount = 5;
    public List<Loot> contents = new List<Loot>();
    public Outline outline;


    // Start is called before the first frame update
    void Start()
    {
        fillContainer();
        printContents();
    }


    public void setFocus(bool value){
        outline.enabled = value;
    }

    public void setFocusEmpty(bool value){
        outline.enabled = value;
        outline.OutlineColor = Color.yellow;
    }

    public bool hasLoot(){
        if(contents.Count > 0) return true;
        return false;
    }

    public void lootAll(List<Loot> targetInventory){
        foreach(Loot loot in contents){
            targetInventory.Add(loot);
        }
        contents.Clear();
    }

    void fillContainer(){
        int itemCount = Random.Range(minItemCount, maxItemCount+1); //add 1 because int version is exclusive on high end
        float roll;
        float currentProbability;

        for(var i = 0; i<itemCount; i++){
            roll = Random.value;
            currentProbability = 0;
            foreach(LootSettings itemSettings in settings.possibleLoot){
                currentProbability += itemSettings.spawnRate;
                if(roll <= currentProbability) contents.Add(itemSettings.loot);
            }
        }
    }

    void printContents(){
        foreach(Loot loot in contents){
            print(loot.name + " " + loot.value);
        }
    }




}

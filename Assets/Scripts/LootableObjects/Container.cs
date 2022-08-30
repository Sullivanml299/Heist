using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public ContainerSettings settings;
    public List<Loot> contents = new List<Loot>();


    // Start is called before the first frame update
    void Start()
    {
        fillContainer();
        printContents();
    }


    void fillContainer(){
        foreach(LootSettings itemSettings in settings.possibleLoot){
            print(itemSettings.loot);
            contents.Add(itemSettings.loot);
        }
    }

    void printContents(){
        foreach(Loot loot in contents){
            print(loot.name + " " + loot.value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class LootSettings 
{
    public Loot loot = null;
    
    [Range(0f, 1f)]
    public float spawnRate = 0.5f;

}

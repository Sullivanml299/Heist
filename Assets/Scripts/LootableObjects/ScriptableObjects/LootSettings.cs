using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class LootSettings 
{
    public Loot loot = null;
    public Texture2D itemIcon = null;         //    What the item will look like in the inventory
    public Rigidbody itemObject = null;       //    Optional slot for a PreFab to instantiate when discarding
    public float spawnRate = 0.5f;

}

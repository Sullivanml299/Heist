using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Loot", order = 1)]
public class Loot : ScriptableObject
{
    public string lootName;
    public int value;
    public Texture2D itemIcon = null;         //    What the item will look like in the inventory
    public Rigidbody itemObject = null;       //    Optional slot for a PreFab to instantiate when discarding
    
}

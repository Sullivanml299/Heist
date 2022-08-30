using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Loot", order = 1)]
public class Loot : ScriptableObject
{
    public string lootName;
    public int value;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnRate_", menuName = "ScriptableObjects/LootSpawnRate", order = 1)]
public class LootSpawnRate : ScriptableObject
{
    public Dictionary<Loot, float> lootRate = new Dictionary<Loot, float>();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContainerSettings_", menuName = "ScriptableObjects/ContainerSettings", order = 1)]
public class ContainerSettings : ScriptableObject
{
    public List<LootSettings> possibleLoot;
}

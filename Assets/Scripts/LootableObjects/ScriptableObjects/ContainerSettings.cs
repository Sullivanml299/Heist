using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContainerSettings_", menuName = "ScriptableObjects/ContainerSettings", order = 1)]
public class ContainerSettings : ScriptableObject
{
    [Header("TODO: add editor code to make this easier")]
    [Header("IMPORTANT: ALL RATES MUST ADD TO 1")]
    public List<LootSettings> possibleLoot;
    public int minItemCount = 1;
    public int maxItemCount = 5;
}

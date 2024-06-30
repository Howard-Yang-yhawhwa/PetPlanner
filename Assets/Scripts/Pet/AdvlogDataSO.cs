using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Advlog", menuName = "ScriptableObjects/Advlog", order = 1)]
[System.Serializable]
public class AdvlogDataSO : ScriptableObject
{
    [Header("=== Basic Info ===")]
    public AdvlogCategory category;
    [TextArea] public string log; 
    // Use @NAME@ to replace with pet name.
    // Use @OTHER@ to replace with other pet name (randomized).

    [Header("=== Additional Contents ===")]
    public List<GiftData> gifts;
    public List<StatsRequirements> requirements;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pet Data", menuName = "ScriptableObjects/Pet", order = 1)]
public class PetDataSO : ScriptableObject
{
    [Header("=== Display Info ===")]
    public string DisplayName;
    [TextArea] public string Description;
    public GameObject ViewPrefab;

    [Space(15)]

    [Header("=== Stats ===")]
    public PetTypes Type;
    public PetRarity Rarity;

    [Header("- Health -")]
    // Reducation Rate Effected By:
    // (FASTER) Happiness
    public float MaxHealth;
    public float MaxHealthReductionDuration;

    [Header("- Hunger -")]
    public float MaxHunger;
    [Tooltip("How long will hunger goes to zero in seconds.")] public float MaxHungerReductionDuration;

    [Header("- Happiness -")]
    // Reduction Rate Effected By:
    // * (+) Hunger
    public float MaxHappiness;
    [Tooltip("How long will happiness goes to zero (under no influence) in seconds.")] public float MaxHappinessReductionDuration;
    [Tooltip("Happiness day time reduction factor.")] public float HappinessDRF = 1f;
    [Tooltip("Happiness night time reduction factor.")] public float HappinessNRF = 0.2f;
}

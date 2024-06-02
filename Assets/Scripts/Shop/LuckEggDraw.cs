using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckEggDraw : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int Cost;

    [Space(20)]
    [Header("Probablities")]
    [SerializeField] float UncommonProbability;
    [SerializeField] float RareProbability;
    [SerializeField] float EpicProbability;
    [SerializeField] float LegendProbability;
    [SerializeField] float MythicProbability;

    Dictionary<PetRarity, List<PetTypes>> petOptions = new Dictionary<PetRarity, List<PetTypes>>();

    private void Start()
    {
        foreach(var kvp in PetManager.AvaliableSOs)
        {
            PetTypes type = kvp.Key;
            PetDataSO dataSO = kvp.Value;

            if (!petOptions.ContainsKey(dataSO.Rarity))
            {
                petOptions.Add(dataSO.Rarity, new List<PetTypes>());
            }

            petOptions[dataSO.Rarity].Add(type);
        }
    }

    public void DrawLuckyEgg()
    {
        if (Player.Currency < Cost)
        {
            return;
        }

        Player.Currency -= Cost;

        float rand = Random.value;
        PetTypes selectedType = DrawPet(PetRarity.Common);

        if (rand < MythicProbability && petOptions.ContainsKey(PetRarity.Mythic))
        {
            DrawPet(PetRarity.Mythic);
            PetSpawner.SpawnPet(selectedType);
            return;
        }

        if (rand < LegendProbability && petOptions.ContainsKey(PetRarity.Legend))
        {
            DrawPet(PetRarity.Legend);
            PetSpawner.SpawnPet(selectedType);
            return;
        }

        if (rand < EpicProbability && petOptions.ContainsKey(PetRarity.Epic))
        {
            DrawPet(PetRarity.Epic);
            PetSpawner.SpawnPet(selectedType);
            return;
        }

        if (rand < RareProbability && petOptions.ContainsKey(PetRarity.Rare))
        {
            DrawPet(PetRarity.Rare);
            PetSpawner.SpawnPet(selectedType);
            return;
        }

        if (rand < UncommonProbability && petOptions.ContainsKey(PetRarity.Uncommon))
        {
            DrawPet(PetRarity.Uncommon);
            PetSpawner.SpawnPet(selectedType);
            return;
        }

        PetSpawner.SpawnPet(selectedType);
        return;

    }

    PetTypes DrawPet(PetRarity rarity)
    {
        return petOptions[rarity][Random.Range(0, petOptions[rarity].Count)];
    }
}

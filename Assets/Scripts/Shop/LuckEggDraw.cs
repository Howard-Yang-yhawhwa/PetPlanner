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
        if (Player.Coins < Cost)
        {
            return;
        }

        float rand = Random.value;
        PetTypes selectedType = DrawPet(PetRarity.Common);

        if (rand < MythicProbability && petOptions.ContainsKey(PetRarity.Mythic))
        {
            selectedType = DrawPet(PetRarity.Mythic);
            EventBus.Publish(new OpenEggDrawConfirmaationEvent(selectedType, Cost));
            return;
        }

        if (rand < LegendProbability && petOptions.ContainsKey(PetRarity.Legendary))
        {
            selectedType = DrawPet(PetRarity.Legendary);
            EventBus.Publish(new OpenEggDrawConfirmaationEvent(selectedType, Cost));
            return;
        }

        if (rand < EpicProbability && petOptions.ContainsKey(PetRarity.Epic))
        {
            selectedType = DrawPet(PetRarity.Epic);
            EventBus.Publish(new OpenEggDrawConfirmaationEvent(selectedType, Cost));
            return;
        }

        if (rand < RareProbability && petOptions.ContainsKey(PetRarity.Rare))
        {
            selectedType = DrawPet(PetRarity.Rare);
            EventBus.Publish(new OpenEggDrawConfirmaationEvent(selectedType, Cost));
            return;
        }

        if (rand < UncommonProbability && petOptions.ContainsKey(PetRarity.Uncommon))
        {
            selectedType = DrawPet(PetRarity.Uncommon);
            EventBus.Publish(new OpenEggDrawConfirmaationEvent(selectedType, Cost));
            return;
        }

        EventBus.Publish(new OpenEggDrawConfirmaationEvent(selectedType, Cost));
        return;

    }

    PetTypes DrawPet(PetRarity rarity)
    {
        // Print the number of pets in the rarity
        Debug.Log($"Number of pets in {rarity}: {petOptions[rarity].Count}");
        return petOptions[rarity][Random.Range(0, petOptions[rarity].Count)];

        // TODO: To make sure player draw more different variation of pet, we will randomize a queue for each rarity
        // and give the player the first pet in the queue whenever they draw the rarity.
        // When we popped half of the queue, we will randomize the queue again.


    }
    
}

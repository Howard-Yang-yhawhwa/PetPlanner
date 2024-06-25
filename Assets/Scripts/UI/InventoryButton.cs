using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text amountText;

    ShopItemTypes type;
    DefaultShopItemSO dataSO;


    public void Setup(ShopItemTypes type)
    {
        this.type = type;
        dataSO = ShopManager.shopItemsMap[type];

        iconImage.sprite = dataSO.icon;
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = $"x{amount}";
    }

    public void OnButtonClicked()
    {
        string currentPetID = PetManager.Instance.SelectedPetID;

        Dictionary<ShopItemTypes, int> tempInventory = Player.Inventory;

        PetData petData = Player.OwnedPets[currentPetID];
        PetDataSO dataReference = PetManager.AvaliableSOs[petData.Type];
        PetController petController = PetManager.SpawnedPets[currentPetID];
        float val = 0;

        if (Player.Inventory[type] <= 0 || !Player.OwnedPets.ContainsKey(currentPetID))
        {
            return;
        }

        bool itemUsed = false;
        int totalExpGained = 0;

        // For Non-stats Related Items
        if (type == ShopItemTypes.Revival)
        {
            if (petData.CurrentHealth <= 0)
            {
                petController.RevivePet();
                itemUsed = true;
            }
        }

        foreach (var effects in dataSO.itemEffects)
        {
            Debug.Log($"Adding effects + {effects.value}{effects.statsType}  to {petData.Nickname}({petData.ID})");
            if(effects.statsType == PetStats.Health)
            {
                float maxHealth = PetManager.AvaliableSOs[petData.Type].MaxHealth;
                if (petData.CurrentHealth == maxHealth) continue; // No need to heal if health is full
                if (petData.CurrentHealth <= 0) continue; // Cannont heal dead pet
                val = effects.isPercentage ? maxHealth * effects.value : effects.value;
                petController.HealPet(val);
                itemUsed = true;
            }

            if (effects.statsType == PetStats.Hunger)
            {
                float maxHunger = PetManager.AvaliableSOs[petData.Type].MaxHunger;
                if (petData.CurrentHunger == maxHunger) continue; // No need to feed if hunger is full
                if (petData.CurrentHealth <= 0) continue; // Cannont feed dead pet
                val = effects.isPercentage ? maxHunger * effects.value : effects.value;
                petController.FeedPet(val);
                itemUsed = true;
            }

            if (effects.statsType == PetStats.Happiness)
            {
                float maxHappiness = PetManager.AvaliableSOs[petData.Type].MaxHappiness;
                if (petData.CurrentHappiness == maxHappiness) continue; // No need to play if happiness is full
                if (petData.CurrentHealth <= 0) continue; // Cannont play with dead pet
                val = effects.isPercentage ? maxHappiness * effects.value : effects.value;
                petController.PlayWithPet(val);
                itemUsed = true;
            }

            if (effects.statsType == PetStats.Experience)
            {
                int maxExp = PetManager.GetMaxExp(petData.Level);
                val = effects.isPercentage ? maxExp * effects.value : effects.value;
                totalExpGained += (int)val;
            }
        }

        if (itemUsed)
        {
            petController.GainExperience(totalExpGained);
            tempInventory[type] -= 1;
            Player.Inventory = tempInventory;
        }

    }
}

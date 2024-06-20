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

        CheckDelete();
    }

    private void Update()
    {
        amountText.text = $"x {Player.Inventory[type]}";
    }

    void CheckDelete()
    {
        if (Player.Inventory[type] <= 0)
        {
            Destroy(gameObject);
        }
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

        foreach (var effects in dataSO.itemEffects)
        {
            Debug.Log($"Adding effects + {effects.value}{effects.statsType}  to {petData.Nickname}({petData.ID})");
            if(effects.statsType == PetStats.Health)
            {
                float maxHealth = PetManager.AvaliableSOs[petData.Type].MaxHealth;
                val = effects.isPercentage ? petData.CurrentHealth * effects.value : effects.value;
                petController.HealPet(val);
            }

            if (effects.statsType == PetStats.Hunger)
            {
                float maxHunger = PetManager.AvaliableSOs[petData.Type].MaxHunger;
                val = effects.isPercentage ? petData.CurrentHunger * effects.value : effects.value;
                petController.FeedPet(val);
            }

            if (effects.statsType == PetStats.Happiness)
            {
                float maxHappiness = PetManager.AvaliableSOs[petData.Type].MaxHappiness;
                val = effects.isPercentage ? petData.CurrentHappiness * effects.value : effects.value;
                petController.PlayWithPet(val);
            }

            if (effects.statsType == PetStats.Experience)
            {
                val = effects.isPercentage ? petData.CurrentExperience * effects.value : effects.value;
                petController.GainExperience(val);
            }
        }

        tempInventory[type] -= 1;

        Player.Inventory = tempInventory;

        CheckDelete();
    }
}

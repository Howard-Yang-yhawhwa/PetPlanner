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
        Dictionary<ShopItemTypes, int> tempInventory = Player.Inventory;
        Dictionary<string, PetData> tempList = Player.OwnedPets;
        float val = 0;

        string currentPetID = PetManager.Instance.SelectedPetID;

        if (Player.Inventory[type] <= 0 || !Player.OwnedPets.ContainsKey(currentPetID))
        {
            return;
        }

        foreach (var effects in dataSO.itemEffects)
        {
            Debug.Log($"Adding effects + {effects.value}{effects.statsType}  to {tempList[currentPetID].Nickname}({tempList[currentPetID].ID})");
            if(effects.statsType == PetStats.Health)
            {
                float maxHealth = PetManager.AvaliableSOs[tempList[currentPetID].Type].MaxHealth;
                val = effects.isPercentage ? tempList[currentPetID].CurrentHealth * effects.value : effects.value;
                tempList[currentPetID].CurrentHealth += val;
                tempList[currentPetID].CurrentHealth = Mathf.Min(maxHealth, tempList[currentPetID].CurrentHealth);
            }

            if (effects.statsType == PetStats.Hunger)
            {
                float maxHunger = PetManager.AvaliableSOs[tempList[currentPetID].Type].MaxHunger;
                val = effects.isPercentage ? tempList[currentPetID].CurrentHunger * effects.value : effects.value;
                tempList[currentPetID].CurrentHunger += val;
                tempList[currentPetID].CurrentHunger = Mathf.Min(maxHunger, tempList[currentPetID].CurrentHunger);
            }

            if (effects.statsType == PetStats.Happiness)
            {
                float maxHappiness = PetManager.AvaliableSOs[tempList[currentPetID].Type].MaxHappiness;
                val = effects.isPercentage ? tempList[currentPetID].CurrentHappiness * effects.value : effects.value;
                tempList[currentPetID].CurrentHappiness += val;
                tempList[currentPetID].CurrentHappiness = Mathf.Min(maxHappiness, tempList[currentPetID].CurrentHappiness);
            }

            if (effects.statsType == PetStats.Experience)
            {
                val = effects.isPercentage ? tempList[currentPetID].CurrentExperience * effects.value : effects.value;

                tempList[currentPetID].CurrentExperience += val;
                if (tempList[currentPetID].CurrentExperience >= PetManager.GetMaxExp(tempList[currentPetID].Level))
                {
                    tempList[currentPetID].Level += 1;
                }
            }
        }

        tempInventory[type] -= 1;

        Player.Inventory = tempInventory;
        Player.OwnedPets = tempList;

        CheckDelete();
    }
}

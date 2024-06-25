using Ricimi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class EggDrawResultDisplay : MonoBehaviour
{
    [SerializeField] Popup popupManager;
    [SerializeField] GameObject newNotificaton;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text rarityText;
    [SerializeField] RawImage dragonImage;
    [SerializeField] GameObject resultDisplay;
    [SerializeField] GameObject giftDisplay;

    [Header("=== Rarity Colors ===")]
    [SerializeField] Color commonColor;
    [SerializeField] Color uncommonColor;
    [SerializeField] Color rareColor;
    [SerializeField] Color epicColor;
    [SerializeField] Color legendaryColor;
    [SerializeField] Color mythicColor;

    string currentPetID;

    public void InitAndOpen(string petID)
    {
        Debug.Log("Egg Draw Result Display Init and Open Called!");

        giftDisplay.SetActive(true);
        resultDisplay.SetActive(false);

        currentPetID = petID;

        PetTypes type = Player.OwnedPets[petID].Type;
        PetDataSO dataSO = PetManager.AvaliableSOs[type];

        // Set Type Display
        string typeStr = StringUtils.FirstCharToUpper(type.ToString());
        typeText.text = $"{typeStr} Dragon";

        // Set Rarity Text and Color
        rarityText.text = StringUtils.FirstCharToUpper(dataSO.Rarity.ToString());
        rarityText.color = dataSO.Rarity switch
        {
            PetRarity.Common => commonColor,
            PetRarity.Uncommon => uncommonColor,
            PetRarity.Rare => rareColor,
            PetRarity.Epic => epicColor,
            PetRarity.Legendary => legendaryColor,
            PetRarity.Mythic => mythicColor,
            _ => commonColor
        };

        // Check if the player has not already owned this type of dragon and show the new notification
        if (!Player.OwnedPets.Any(p => p.Value.Type == type))
        {
            newNotificaton.SetActive(true);
        }

        // Display the dragon image
        dragonImage.texture = dataSO.AnimatedIcon;
        if (AnimatedIconsManager.Instance == null)
        {
            Debug.LogError($"AnimatedIconsManager is null in {gameObject.name}!");
            return;
        }
        AnimatedIconsManager.Instance.DisplayAnimatedIcon(type);
        

        popupManager.Open();

    }

    public void ShowResult()
    {
        resultDisplay.SetActive(true);
        giftDisplay.SetActive(false);
    }

    public void ConfirmResult()
    {
        // Open name change panel with event bus
        EventBus.Publish(new OpenEditPetNameWindowEvent(currentPetID));
        popupManager.Close();
        if (AnimatedIconsManager.Instance == null)
        {
            Debug.LogError($"AnimatedIconsManager is null in {gameObject.name}!");
            return;
        }
        AnimatedIconsManager.Instance.HideAllAnimatedIcons();
    }
}

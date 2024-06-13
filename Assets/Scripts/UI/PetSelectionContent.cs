using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PetSelectionContent : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] public Button selectButton;
    [SerializeField] Image iconImage;
    [SerializeField] RawImage animatedIconImage;
    [SerializeField] Image[] iconBackgroundImage;

    [Header("=== Rarity Display Color ===")]
    [SerializeField] Color[] rarityColors;
    


    GameObject parentUI;
    PetData currPetData;

    private void Start()
    {
        selectButton.onClick.AddListener(OnButtonClicked);
    }

    public void UpdateDisplay(string id, GameObject parentUI)
    {
        this.parentUI = parentUI;
        currPetData = Player.OwnedPets[id];
        nameText.text = $"{currPetData.Nickname}";

        iconImage.sprite = PetManager.AvaliableSOs[currPetData.Type].DisplayIcon;
        iconImage.gameObject.SetActive(true);
        animatedIconImage.gameObject.SetActive(false);

        // If animated icon is available, display it and hide the static icon
        if (PetManager.AvaliableSOs[currPetData.Type].AnimatedIcon != null)
        {
            animatedIconImage.texture = PetManager.AvaliableSOs[currPetData.Type].AnimatedIcon;
            iconImage.gameObject.SetActive(false);
            animatedIconImage.gameObject.SetActive(true);
        }

        foreach(Image background in iconBackgroundImage)
        {
            background.color = rarityColors[(int)PetManager.AvaliableSOs[currPetData.Type].Rarity];
        }
    }

    public void OnButtonClicked()
    {
        // parentUI.SetActive(false);
        // EventBus.Publish(new PetSelectedEvent(currPetData.ID));
        EventBus.Publish(new PetSelectedEvent(currPetData.ID));
    }
}

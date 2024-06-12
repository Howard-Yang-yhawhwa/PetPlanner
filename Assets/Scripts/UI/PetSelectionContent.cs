using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PetSelectionContent : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] public Button selectButton;

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
    }

    public void OnButtonClicked()
    {
        // parentUI.SetActive(false);
        // EventBus.Publish(new PetSelectedEvent(currPetData.ID));
        EventBus.Publish(new PetSelectedEvent(currPetData.ID));
    }
}

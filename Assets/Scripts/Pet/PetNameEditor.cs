using Ricimi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetNameEditor : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] Button confirmButton;
    [SerializeField] Popup popupManager;

    string targetPetID;

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    public void InitAndOpen(string petID)
    {
        targetPetID = petID;
        nameInput.text = "";
        popupManager.Open();
    }

    public void OnConfirmButtonClicked()
    {
        // Return if the inputfield is empty
        if (string.IsNullOrEmpty(nameInput.text))
        {
            return;
        }

        // Check if petID exists in owned pets
        if (Player.OwnedPets.ContainsKey(targetPetID))
        {
            // Update the pet's name
            Player.OwnedPets[targetPetID].Nickname = nameInput.text;
        }
        else
        {
            Debug.LogError("Pet with ID " + targetPetID + " not found in owned pets.");
        }

        // Close the panel
        popupManager.Close();
    }
}

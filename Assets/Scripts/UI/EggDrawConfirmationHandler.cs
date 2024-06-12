using Ricimi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDrawConfirmationHandler : MonoBehaviour
{
    [SerializeField] Popup popupManager;
    [SerializeField] TMPro.TMP_Text costText;


    private PetTypes selectedType;
    private int cost;

    Subscription<OpenEggDrawConfirmaationEvent> egg_draw_event;

    private void Awake()
    {
        egg_draw_event = EventBus.Subscribe<OpenEggDrawConfirmaationEvent>(OnEggDrawEvent);
        popupManager.Close();
    }

    void OnEggDrawEvent(OpenEggDrawConfirmaationEvent e)
    {
        SetupPanel(e.selectedType, e.cost);
    }

    public void SetupPanel(PetTypes type, int cost)
    {
        selectedType = type;
        this.cost = cost;

        costText.text = cost.ToString();

        popupManager.Open();
    }

    public void OnConfirmButtonClicked()
    {
        Player.Currency -= cost;
        string newPetID = PetSpawner.SpawnNewPet(selectedType);

        // Open name change panel with event bus
        EventBus.Publish(new OpenEditPetNameWindowEvent(newPetID));

        // TODO: Show lucky egg draw animation
        // TODO: Show naming panel after the egg draw animation
        popupManager.Close();
    }

    public void OnDeclinedButtonClicked()
    {
        popupManager.Close();
    }
    
}

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
    private Sprite currentEggDrawSprite;

    Subscription<OpenEggDrawConfirmaationEvent> egg_draw_event;

    private void Awake()
    {
        egg_draw_event = EventBus.Subscribe<OpenEggDrawConfirmaationEvent>(OnEggDrawEvent);
    }

    void OnEggDrawEvent(OpenEggDrawConfirmaationEvent e)
    {
        SetupPanel(e.selectedType, e.cost, e.eggDrawSprite);
    }

    public void SetupPanel(PetTypes type, int cost, Sprite eggDrawSprite)
    {
        selectedType = type;
        this.cost = cost;
        currentEggDrawSprite = eggDrawSprite;

        costText.text = cost.ToString();

        popupManager.Open();
    }

    public void OnConfirmButtonClicked()
    {
        Player.Coins -= cost;
        string newPetID = PetSpawner.SpawnNewPet(selectedType);
        
        // Open the egg draw result display
        EventBus.Publish(new OpenDrawPetResultEvent(newPetID, currentEggDrawSprite));

        // TODO: Show lucky egg draw animation
        // TODO: Show naming panel after the egg draw animation
        popupManager.Close();
    }

    public void OnDeclinedButtonClicked()
    {
        popupManager.Close();
    }
    
}

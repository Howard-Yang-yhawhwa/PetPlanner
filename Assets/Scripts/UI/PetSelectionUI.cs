using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

public class PetSelectionUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] PetSelectionContent contentPrefab;

    Subscription<OwnedPetsUpdateEvent> pet_updated_event;

    Popup popupManager;

    Dictionary<string, PetSelectionContent> buttonMapping = new Dictionary<string, PetSelectionContent>();

    private void Awake()
    {
        pet_updated_event = EventBus.Subscribe<OwnedPetsUpdateEvent>(OnOwnedPetsUpdateEvent);

        popupManager = GetComponent<Popup>();
    }
    private void Start()
    {
        UpdateDisplay();
    }


    void OnOwnedPetsUpdateEvent(OwnedPetsUpdateEvent e)
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        foreach(var kvp in Player.OwnedPets)
        {
            string id = kvp.Key;
            PetData data = kvp.Value;

            if (!buttonMapping.ContainsKey(id))
            {
                PetSelectionContent clone = Instantiate(contentPrefab, container);
                clone.selectButton.onClick.AddListener(popupManager.Close);
                buttonMapping.Add(id, clone);
            }

            buttonMapping[id].UpdateDisplay(id, gameObject);
        }
    }
}

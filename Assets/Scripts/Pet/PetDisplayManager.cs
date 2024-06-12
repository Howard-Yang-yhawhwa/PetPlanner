using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetDisplayManager : MonoBehaviour
{

    private Dictionary<string, PetController> petDictionary = new Dictionary<string, PetController>();
    string currentDisplayingPetID;

    Subscription<PetSelectedEvent> pet_selected_event;

    Subscription<PetSpawnedEvent> pet_spawned_event;

    private void Awake()
    {
        pet_selected_event = EventBus.Subscribe<PetSelectedEvent>(OnPetSelectedEvent);
        pet_spawned_event = EventBus.Subscribe<PetSpawnedEvent>(OnPetSpawnedEvent);
    }

    void OnPetSpawnedEvent(PetSpawnedEvent e)
    {
        GetPets();
    }

    void OnPetSelectedEvent(PetSelectedEvent e)
    {
        currentDisplayingPetID = e.petID;
        HideAllPets();
        petDictionary[currentDisplayingPetID].gameObject.SetActive(true);
    }

    void HideAllPets()
    {
        foreach (PetController pet in petDictionary.Values)
        {
            pet.gameObject.SetActive(false);
        }
    }

    public void GetPets()
    {
        petDictionary.Clear();

        foreach (Transform child in transform)
        {
            PetController controller;
            if (child.gameObject.TryGetComponent(out controller))
            {
                petDictionary.Add(controller.ID, controller);
            }
        }
    }
}

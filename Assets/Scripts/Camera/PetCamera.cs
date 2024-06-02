using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PetCamera : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook petCamera;

    string petID;

    Subscription<PetSelectedEvent> selected_event;


    private void Awake()
    {
        selected_event = EventBus.Subscribe<PetSelectedEvent>(OnPetSelected);
        petCamera.Priority = 0;
    }

    void OnPetSelected(PetSelectedEvent e)
    {
        if (e.petID != petID)
        {
            // Things to do if this pet IS NOT selected
            petCamera.Priority = 0;
            return;
        }

        // Things to do if this pet IS selected
        petCamera.Priority = 100;
    }

    public void Setup(string id)
    {
        petID = id;
    }
}

using Ricimi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EditPetNameWindowHandler : MonoBehaviour
{
    [SerializeField] PetNameEditor editor;

    Subscription<OpenEditPetNameWindowEvent> edit_name_event;

    string currentPetID;

    private void Awake()
    {
        edit_name_event = EventBus.Subscribe<OpenEditPetNameWindowEvent>(OnEditPetNameEvent);
    }

    void OnEditPetNameEvent(OpenEditPetNameWindowEvent e)
    {
        currentPetID = e.petID;
        editor.InitAndOpen(e.petID);
    }

}

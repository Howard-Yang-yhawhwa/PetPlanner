using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtasksUpdateEvent { }
public class OpenEggDrawConfirmaationEvent
{
    public PetTypes selectedType;
    public int cost;

    public OpenEggDrawConfirmaationEvent(PetTypes selectedType, int cost)
    {
        this.selectedType = selectedType;
        this.cost = cost;
    }
}

public class OpenEditPetNameWindowEvent 
{
    public string petID;

    public OpenEditPetNameWindowEvent(string petID)
    {
        this.petID = petID;
    }
}

public class OpenDrawPetResultEvent
{
    public string petID;

    public OpenDrawPetResultEvent(string petID)
    {
        this.petID = petID;
    }
}

public class CloseAllUIEvent {}
public class DisplayBottomBarEvent 
{
    public bool shouldDisplay;

    public DisplayBottomBarEvent(bool shouldDisplay)
    {
        this.shouldDisplay = shouldDisplay;
    }
}
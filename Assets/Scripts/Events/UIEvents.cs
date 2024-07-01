using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtasksUpdateEvent { }
public class OpenEggDrawConfirmaationEvent
{
    public PetTypes selectedType;
    public int cost;
    public Sprite eggDrawSprite;

    public OpenEggDrawConfirmaationEvent(PetTypes selectedType, int cost, Sprite eggDrawSprite)
    {
        this.selectedType = selectedType;
        this.cost = cost;
        this.eggDrawSprite = eggDrawSprite;
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
    public Sprite eggDrawSprite;
    public OpenDrawPetResultEvent(string petID, Sprite eggDrawSprite)
    {
        this.petID = petID;
        this.eggDrawSprite = eggDrawSprite;
    }
}

public class OpenGiftRecievedPopupEvent
{
    public List<GiftData> gifts;

    public OpenGiftRecievedPopupEvent(List<GiftData> gifts)
    {
        this.gifts = gifts;
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
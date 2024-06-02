using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyUpdateEvent
{
    public int newValue;
    public int deltaValue;

    public CurrencyUpdateEvent(int newValue, int deltaValue)
    {
        this.newValue = newValue;
        this.deltaValue = deltaValue;
    }
}

public class InventoryUpdateEvent { }

public class SetEditTaskEvent 
{
    public string taskID;
    public SetEditTaskEvent(string taskID)
    {
        this.taskID = taskID;
    }
}
public class UpdateTaskList { }

public class PetSelectionMadeEvent
{
    public string petID;
    public PetSelectionMadeEvent(string petID)
    {
        this.petID = petID;
    }
}

public class PetSelectedEvent
{
    public GameObject petObject;
    public string petID;

    public PetSelectedEvent(GameObject petObject, string petID)
    {
        this.petObject = petObject;
        this.petID = petID;
    }
}

public class OwnedPetsUpdateEvent { }

public class PetStatsUpdateEvent
{
    public string petID;

    public PetStatsUpdateEvent(string petID)
    {
        this.petID = petID;
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solstice.Core;

public class Player : MonoBehaviour
{
    [SerializeField] float SavePeriod;

    [Space(30)]
    [Header("=== Debug Info ===")]
    [SerializeField] bool DebugMode;
    [SerializeField] List<ShopItemTypes> InventoryTypes;
    [SerializeField] List<int> InventoryCount;

    public static int Currency {
        get {
            return currency;
        }
        set {
            EventBus.Publish(new CurrencyUpdateEvent(value, value - currency));
            currency = value;
        }
    }
    static int currency;

    public static Dictionary<ShopItemTypes, int> Inventory {

        get
        {
            return inventory;
        }

        set
        {
            inventory = value;
            EventBus.Publish(new InventoryUpdateEvent());
        }
    }
    static Dictionary<ShopItemTypes, int> inventory = new Dictionary<ShopItemTypes, int>();

    public static Dictionary<string, PetData> OwnedPets {
        get {
            return ownedPets;
        }

        set {
            ownedPets = value;
            EventBus.Publish(new OwnedPetsUpdateEvent());
        }
    }
    static Dictionary<string, PetData> ownedPets = new Dictionary<string, PetData>();

    public static long LastLoginTime;

    public static void AddItem(ShopItemTypes type)
    {
        Dictionary<ShopItemTypes, int> tempList = Inventory;

        if (!tempList.ContainsKey(type))
        {
            tempList.Add(type, 0);
        }

        Debug.Log($"Add {type} to inventory!");
        tempList[type]++;

        Inventory = tempList;
    }
    public static void RemoveItem(ShopItemTypes type)
    {

        Dictionary<ShopItemTypes, int> tempList = Inventory;

        if (!tempList.ContainsKey(type))
        {
            Debug.LogError("Item does not exist!");
            return;
        }

        if (tempList[type] == 0)
        {
            Debug.LogError("Item count = 0, should not be able to use!");
            return;
        }

        tempList[type]--;

        Inventory = tempList;
    }

    float lastSavedTime;

    private void Awake()
    {
        LoadData();
    }

    private void Update()
    {
        if (Time.time - lastSavedTime > SavePeriod)
        {
            lastSavedTime = Time.time;
            SaveData();
        }

        if (DebugMode)
        {
            InventoryCount.Clear();
            InventoryTypes.Clear();
            foreach(var kvp in Inventory)
            {
                InventoryTypes.Add(kvp.Key);
                InventoryCount.Add(kvp.Value);
            }
        }
    }

    private void OnApplicationQuit()
    {
        LastLoginTime = TimeUtils.GetTimestamp();
        SaveData();
    }

    public static void LoadData()
    {
        currency = SaveManager.Load<int>("Player_Currency");

        ownedPets = SaveManager.Load<Dictionary<string, PetData>>("Player_OwnedPets");
        if (ownedPets == null)
        {
            ownedPets = new Dictionary<string, PetData>();
            SaveManager.Save("Player_OwnedPets", ownedPets);
        }

        inventory = SaveManager.Load<Dictionary<ShopItemTypes, int>>("Player_Inventory");
        if (inventory == null)
        {
            inventory = new Dictionary<ShopItemTypes, int>();
            SaveManager.Save("Player_Inventory", inventory);
        }

        LastLoginTime = SaveManager.Load<long>("Last_Login_Time");
        if (LastLoginTime == 0)
        {
            LastLoginTime = TimeUtils.GetTimestamp();
            SaveManager.Save("Last_Login_Time", LastLoginTime);
        }

    }

    public static void SaveData()
    {
        Debug.Log($"PLAYER DATA SAVED @ {Time.time}");

        SaveManager.Save("Player_Currency", currency);
        currency = SaveManager.Load<int>("Player_Currency");

        SaveManager.Save("Player_OwnedPets", ownedPets);
        ownedPets = SaveManager.Load<Dictionary<string, PetData>>("Player_OwnedPets");

        SaveManager.Save("Player_Inventory", Inventory);
        inventory = SaveManager.Load<Dictionary<ShopItemTypes, int>>("Player_Inventory");

        SaveManager.Save("Last_Login_Time", LastLoginTime);
    }
}

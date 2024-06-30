using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solstice.Core;
using Unity.VisualScripting;

public enum PetTypes {  
                        amberhorn, boulder, breeze, cloudling, duskwind, meadow, splash, // Common
                        blaze, candycorn, hypno, oddball, phantom, venomfang, // Uncommon
                        dizzy, goblinfire, hellbound, pearl, rainbowhorn, // Rare
                        doodle, reaper, valentine, weepy, // Epic
                        fortune, inferno, unicorn, // Legendary
                        eclipse, eldritch, nightfury // Mythic
                     }
public enum PetStats { Health, Hunger, Happiness, Experience, Others }
public enum PetRarity { Common, Uncommon, Rare, Epic, Legendary, Mythic }
public enum PetState { Idling, Resting, Roaming, Eating, Celebrating, Death }

public class PetManager : MonoBehaviour
{
    public static PetManager Instance;

    [SerializeField] List<PetDataSO> AvaliablePetScriptableObjects;

    [Header("=== DEBUG INFO ===")]
    [SerializeField] bool DebugMode;
    [SerializeField] List<PetData> OwnedPetData;
    [SerializeField] List<PetController> SpawnedPetList;
    [SerializeField] public string SelectedPetID = "None";

    public static Dictionary<PetTypes, PetDataSO> AvaliableSOs = new Dictionary<PetTypes, PetDataSO>();
    public static Dictionary<string, PetController> SpawnedPets = new Dictionary<string, PetController>();

    static int[] maxExpMemo = new int[101];

    public static readonly int AdvlogPeriod = 10800; // 3 hours

    Subscription<PetSelectedEvent> selected_event;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        selected_event = EventBus.Subscribe<PetSelectedEvent>(OnSelectedEvent);

        foreach (PetDataSO so in AvaliablePetScriptableObjects)
        {
            AvaliableSOs.Add(so.Type, so);
        }


        // Populate max experince needed for each level
        maxExpMemo[0] = 1000;
        maxExpMemo[1] = 1000;

        for(int i = 2; i < maxExpMemo.Length; i ++)
        {
            maxExpMemo[i] = (int)(maxExpMemo[i - 1] * 1.25f);
        }
    }

    public void Update()
    {
        if (DebugMode)
        {
            OwnedPetData.Clear();
            foreach (var kvp in Player.OwnedPets)
            {
                string id = kvp.Key;
                PetData data = kvp.Value;
                OwnedPetData.Add(data);
            }

            SpawnedPetList.Clear();
            foreach (var controller in SpawnedPets.Values)
            {
                SpawnedPetList.Add(controller);
            }
        }

    }

    void OnSelectedEvent(PetSelectedEvent e)
    {
        SelectedPetID = e.petID;
    }

    public static int GetMaxExp(int level)
    {
        // return 1000 * (int)Mathf.Pow(2, level - 1);
        return maxExpMemo[level];
    }

    public static PetData CreateData(PetTypes type)
    {
        string ID = RandomUtils.GenerateNumericCode(8);
        return new PetData(AvaliableSOs[type], ID);
    }

    public static float CalculateReductionPeriod(PetStats statsType, PetData data)
    {
        PetDataSO templateData = AvaliableSOs[data.Type];
        float normalRP = 0;

        switch (statsType)
        {
            case PetStats.Health:
                if (data.CurrentHunger > 0 || data.CurrentHealth <= 0)
                {
                    return 0;
                }

                float happinessFMC = 2; // Health will drop at most <x> times as fast depending on happiness

                normalRP = templateData.MaxHealthReductionDuration / templateData.MaxHealth;
                float happinessFactor = 1 / (happinessFMC * (1 - (data.CurrentHappiness / templateData.MaxHappiness)));
                
                return normalRP * happinessFactor;

            case PetStats.Hunger:

                if (data.CurrentHunger <= 0)
                {
                    return 0;
                }

                normalRP = templateData.MaxHungerReductionDuration / templateData.MaxHunger;
                return normalRP;

            case PetStats.Happiness:

                if (data.CurrentHappiness <= 0)
                {
                    return 0;
                }

                float hungerFMC = 2; // Happiness will drop at most <x> times as fast depending on hunger
                
                normalRP = (templateData.MaxHappinessReductionDuration / templateData.MaxHappiness);
                float dayNightFactor = (1 / (TimeUtils.isNighTime() ? templateData.HappinessDRF : templateData.HappinessNRF));
                float hungerFactor = 1 / (hungerFMC * (1 - (data.CurrentHunger / templateData.MaxHunger)));
                return normalRP * hungerFactor;

            default:
                return 0;
        }
    }
    public static void AddAdvlogToPet(string petID, AdvlogData data)
    {
        Dictionary<string, PetData> tempList = Player.OwnedPets;
        while (tempList[petID].myAdvlogs.Count > 15)
        {
            tempList[petID].myAdvlogs.RemoveAt(0);
        }
        tempList[petID].myAdvlogs.Add(data);
    }
}

[System.Serializable]
public class PetData
{
    public string ID;
    public PetTypes Type;
    public string Nickname;
    public int Level;
    public float CurrentHealth;
    public float CurrentHunger;
    public float CurrentHappiness;
    public float CurrentExperience;
    public List<AdvlogData> myAdvlogs;

    // Time Related Data
    public long CreationTime;
    public long LastGeneralAdvlogTime = 0;
    public float HealthCumulativeTime;
    public float HungerCumulativeTime;
    public float HappinessCumulativeTime;
    public float AdvlogCumulativeTime;

    public PetData(PetDataSO so, string ID)
    {
        Nickname = so.DisplayName;
        Level = 1;
        CurrentHealth = so.MaxHealth;
        CurrentHunger = so.MaxHunger;
        CurrentHappiness = so.MaxHappiness / 2;
        CurrentExperience = 0;
        Type = so.Type;
        this.ID = ID;
        CreationTime = TimeUtils.GetTimestamp();
        HealthCumulativeTime = 0;
        HungerCumulativeTime = 0;
        HappinessCumulativeTime = 0;
        LastGeneralAdvlogTime = TimeUtils.GetTimestamp();

        myAdvlogs = new List<AdvlogData>();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


public enum AdvlogCategory { 
    [Description("General Advlog")] General,
    [Description("Gift Relalted Advlog")] Gift,
    [Description("Rest & Sleep Related")] Rest,
    [Description("Feeding Related Advlog")] Food,
    [Description("Health Related Advlog")] Health,
    [Description("Playing Related Advlog")] Happiness,
    [Description("Level Related Advlog")] Experience,
    [Description("Birth Event Advlog")] Birth,
    [Description("Death Events Advlog")] Death,
}

[System.Serializable]
public class GiftData 
{
    public ShopItemTypes type;
    public int amount;
}

[System.Serializable]
public class StatsRequirements 
{
    public PetStats stats;

    // Min/Max values the pet stats should be in to trigger this log (in percentage)
    // * Every stats expect Experience will be in percentage
    // * Experinece will be levels
    public float minValue; 
    public float maxValue;
}


[System.Serializable]
public class AdvlogData 
{
    public string SOName;
    public string timestamp;

    public AdvlogData(string SOName, string timestamp)
    {
        this.SOName = SOName;
        this.timestamp = timestamp;
    }

    public override string ToString()
    {
        AdvlogDataSO dataSO = AdvlogManager.Instance.NameLookup[SOName];
        return $"Timestamp: {timestamp}\n" +
            $"Category - {dataSO.category}\n" +
            $"Log - {dataSO.log}\n" +
            $"Have requirements - {dataSO.requirements.Count > 0}\n" +
            $"Have gifts - {dataSO.gifts.Count > 0}";
    }
}


public class AdvlogManager : MonoBehaviour
{
    public static AdvlogManager Instance;

    [SerializeField] List<AdvlogDataSO> AvaliableDataSO;
    [SerializeField] bool debugMode;

    Dictionary<AdvlogCategory, List<AdvlogDataSO>> categoryLookup = new Dictionary<AdvlogCategory, List<AdvlogDataSO>>();
    public Dictionary<string, AdvlogDataSO> NameLookup = new Dictionary<string, AdvlogDataSO>();

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

        // Store all data in a category dictionary for easy lookup
        foreach (AdvlogDataSO so in AvaliableDataSO)
        {
            if (!categoryLookup.ContainsKey(so.category))
            {
                categoryLookup.Add(so.category, new List<AdvlogDataSO>());
            }
            categoryLookup[so.category].Add(so);

            NameLookup.Add(so.name, so);
        }
    }

    private void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AdvlogData data = GetAdvlog(AdvlogCategory.General, TimeUtils.GetTimestamp());
                if (data != null) Debug.Log(data);
            }
        }
    }

    public AdvlogData GetAdvlog(AdvlogCategory category, long timestamp, PetData petData = null)
    {
        List<AdvlogDataSO> advlogList = categoryLookup[category];
        List<AdvlogDataSO> validLogs = new List<AdvlogDataSO>();

        if (petData != null)
        {
            PetDataSO dataSO = PetManager.AvaliableSOs[petData.Type];
            Dictionary<PetStats, float> stats = new Dictionary<PetStats, float>();
            stats.Add(PetStats.Health, petData.CurrentHealth / dataSO.MaxHealth);
            stats.Add(PetStats.Hunger, petData.CurrentHunger / dataSO.MaxHunger);
            stats.Add(PetStats.Happiness, petData.CurrentHappiness / dataSO.MaxHappiness);
            stats.Add(PetStats.Experience, petData.Level);
            
            // TODO: Filter out logs based on petData and requirements
            foreach(AdvlogDataSO so in advlogList)
            {
                bool valid = true;
                foreach (StatsRequirements requirement in so.requirements)
                {
                    if (!stats.ContainsKey(requirement.stats) || stats[requirement.stats] > requirement.maxValue || stats[requirement.stats] < requirement.minValue)
                    {
                        valid = false;
                    }
                }

                if (valid) validLogs.Add(so);
                
            }
        }
        else
        {
            validLogs = advlogList;
        }

        // Randomly pick a log from the valid logs list
        if (validLogs.Count <= 0)
        {
            Debug.LogWarning($"No valid logs found for category {category}");
            return null;
        }

        int randomIndex = Random.Range(0, validLogs.Count);
        Debug.Log($"Choosing a random index from 0 to {validLogs.Count}...Chose: {randomIndex}");
        AdvlogDataSO selectedLog = validLogs[randomIndex];

        // Convert Timestamp into Human Readable format (yyyy-MM-dd HH:mm:ss)
        string timestampStr = TimeUtils.ConvertTimestamp(timestamp);

        return new AdvlogData(selectedLog.name, timestampStr);
    }
}

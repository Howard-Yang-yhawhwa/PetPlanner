using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Solstice.Core;

[RequireComponent(typeof(PetViewController))]
public class PetController : MonoBehaviour
{
    [Header("PET SETTINGS")]
    [Space(5)]

    [Header("============= Idle =============")]
    [SerializeField] float IdleMinTime;
    [SerializeField] float IdleMaxTime;
    [Space(15)]

    [Header("============= Roaming =============")]
    [SerializeField] float RoamMinTime;
    [SerializeField] float RoamMaxTime;
    [SerializeField] float RoamSpeed;

    [Space(30)]
    [Header("DEBUG INFO")]
    [SerializeField] bool initialized = false;
    [SerializeField] float timer = 0;
    [SerializeField] PetState currentState;
    [SerializeField] float currentStateDuration;
    [SerializeField] GameObject viewObject;

    // NavMeshAgent agent;

    public string ID
    {
        get { return id;  }
        private set { id = value; }
    }

    string id;
    float previousTimestamp;
    bool selected = false;

    PetDataSO dataSO;
    PetViewController viewController;
    PetAnimator petAnimator;
    Rigidbody rigid;

    Subscription<PetSelectedEvent> selection_event;

    private void Awake()
    {
        selected = false;
        selection_event = EventBus.Subscribe<PetSelectedEvent>(OnSelectionMade);
    }

    void OnSelectionMade(PetSelectedEvent e)
    {
        if (e.petID != id) {
            selected = false;
            return; 
        }

        selected = true;
        viewController.UpdateDisplay(Player.OwnedPets[ID], false);

        Debug.Log($"{gameObject.name} for pet {e.petID} SELECTED!");
    }

    private void Start()
    {
        currentState = PetState.Idling;
        timer = 0;

        rigid = GetComponent<Rigidbody>();
        
    }


    // For spawning existing pet
    public void Initialize(string ID)
    {
        this.ID = ID;
        dataSO = PetManager.AvaliableSOs[Player.OwnedPets[ID].Type];

        gameObject.name = $"{dataSO.Rarity} - {dataSO.Type} - {Player.OwnedPets[ID].Nickname} ({ID})";

        viewController = GetComponent<PetViewController>();
        petAnimator = GetComponent<PetAnimator>();
        // agent = GetComponent<NavMeshAgent>();

        Debug.Log($"Data SO's view prefab: {dataSO.ViewPrefab}");

        viewObject = viewController.InitializeDisplay(dataSO.ViewPrefab);
        petAnimator.SetupAniamtor(viewObject.GetComponent<Animator>());

        long currentTime = TimeUtils.GetTimestamp();
        Debug.Log($"Creation Time: {Player.OwnedPets[ID].CreationTime} | Last Login Time: {Player.LastLoginTime}");
        long previousTime = Player.OwnedPets[ID].CreationTime > Player.LastLoginTime ? Player.OwnedPets[ID].CreationTime : Player.LastLoginTime;
        long deltaTime = currentTime - previousTime;
        Debug.Log($"{gameObject.name} checking prev time: {previousTime}, delta time: {deltaTime}");

        Dictionary<string, PetData> tempList = Player.OwnedPets;

        tempList[ID].HealthCumulativeTime += deltaTime;
        tempList[ID].HungerCumulativeTime += deltaTime;
        tempList[ID].HappinessCumulativeTime += deltaTime;

        Player.OwnedPets = tempList;

        previousTimestamp = Time.time;

        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;

        float currentTimestamp = TimeUtils.GetTimestamp();

        timer += Time.deltaTime;

        RunStateMachine();

        float deltaTime = Time.time - previousTimestamp;
        previousTimestamp = Time.time;

        Dictionary<string, PetData> tempList = Player.OwnedPets;

        // Update Timers
        tempList[ID].HealthCumulativeTime = tempList[ID].CurrentHealth > 0 ? tempList[ID].HealthCumulativeTime + deltaTime : 0;
        tempList[ID].HungerCumulativeTime = tempList[ID].CurrentHunger > 0 ? tempList[ID].HungerCumulativeTime + deltaTime : 0;
        tempList[ID].HappinessCumulativeTime = tempList[ID].CurrentHappiness > 0 ? tempList[ID].HappinessCumulativeTime + deltaTime : 0;

        // Hunger Decay
        bool changesMade = false;
        float hungerRP = PetManager.CalculateReductionPeriod(PetStats.Hunger, tempList[ID]);

        if (hungerRP != 0 && tempList[id].HungerCumulativeTime >= hungerRP)
        {
            tempList[id].CurrentHunger -= 1;

            if (tempList[id].CurrentHunger <= 0)
            {
                tempList[id].HungerCumulativeTime = 0;
                tempList[id].CurrentHunger = 0;
            }
            else
            {
                tempList[id].HungerCumulativeTime -= hungerRP;
            }
            
            changesMade = true;
        }

        // Happiness Decay
        float happinessRP = PetManager.CalculateReductionPeriod(PetStats.Happiness, tempList[ID]);

        if (happinessRP != 0 && tempList[id].HappinessCumulativeTime >= happinessRP)
        {
            tempList[id].CurrentHappiness -= 1;
            if (tempList[id].CurrentHappiness <= 0)
            {
                tempList[id].HappinessCumulativeTime = 0;
                tempList[id].CurrentHappiness = 0;
            }
            else
            {
                tempList[id].HappinessCumulativeTime -= happinessRP;
            }
            
            changesMade = true;
        }

        // Health Decay
        float healthRP = PetManager.CalculateReductionPeriod(PetStats.Health, tempList[ID]);

        if (healthRP != 0 && tempList[id].HealthCumulativeTime >= healthRP)
        {
            tempList[id].CurrentHealth -= 1;
            if (tempList[id].CurrentHealth <= 0)
            {
                tempList[id].HealthCumulativeTime = 0;
                tempList[id].CurrentHealth = 0;
            }
            tempList[id].HealthCumulativeTime -= healthRP;
            changesMade = true;
        }

        if (changesMade)
        {
            EventBus.Publish(new PetStatsUpdateEvent(id));
        }

        Player.OwnedPets = tempList;

    }

    void RunStateMachine()
    {
        switch (currentState)
        {
            case PetState.Idling:
                Idling();
                break;
            case PetState.Roaming:
                Roaming();
                break;
        }
    }

    void SwitchState(PetState newState)
    {
        float minVal = 0;
        float maxVal = 0;

        switch (currentState)
        {
            case PetState.Idling:
                minVal = IdleMinTime;
                maxVal = IdleMaxTime;
                break;
            case PetState.Roaming:
                minVal = RoamMinTime;
                maxVal = RoamMaxTime;
                break;
        }

        currentStateDuration = UnityEngine.Random.Range(minVal, maxVal);
        currentState = newState;
    }

    void Idling()
    {

        // agent.SetDestination(transform.position);

        if (timer > currentStateDuration)
        {
            timer = 0;
            SwitchState(PetState.Roaming);
        }
    }

    void Roaming()
    {

        if (timer > currentStateDuration)
        {
            timer = 0;
            SwitchState(PetState.Idling);
        }
    }

    public void FeedPet(float foodValue)
    {
        petAnimator.PlayEatingAnimation();
        Dictionary<string, PetData> tempList = Player.OwnedPets;

        tempList[ID].CurrentHunger += foodValue;
        tempList[ID].CurrentHunger = Mathf.Min(dataSO.MaxHunger, tempList[ID].CurrentHunger);

        Player.OwnedPets = tempList;
    }

    public void PlayWithPet(float happinessValue)
    {
        petAnimator.PlayCheeringAnimation();
        Dictionary<ShopItemTypes, int> tempInventory = Player.Inventory;
        Dictionary<string, PetData> tempList = Player.OwnedPets;

        tempList[ID].CurrentHappiness += happinessValue;
        tempList[ID].CurrentHappiness = Mathf.Min(dataSO.MaxHappiness, tempList[ID].CurrentHappiness);

        Player.OwnedPets = tempList;
    }

    public void HealPet(float healthValue)
    {
        petAnimator.PlayCheeringAnimation();
        Dictionary<ShopItemTypes, int> tempInventory = Player.Inventory;
        Dictionary<string, PetData> tempList = Player.OwnedPets;

        tempList[ID].CurrentHealth += healthValue;
        tempList[ID].CurrentHealth = Mathf.Min(dataSO.MaxHealth, tempList[ID].CurrentHealth);

        Player.OwnedPets = tempList;
    }

    public void GainExperience(float experienceValue)
    {
        Dictionary<ShopItemTypes, int> tempInventory = Player.Inventory;
        Dictionary<string, PetData> tempList = Player.OwnedPets;

        tempList[ID].CurrentExperience += experienceValue;
        if (tempList[ID].CurrentExperience >= PetManager.GetMaxExp(tempList[ID].Level))
        {
            viewController.UpdateDisplay(Player.OwnedPets[ID], true);
            tempList[ID].Level += 1;
        }

        Player.OwnedPets = tempList;
    }
}




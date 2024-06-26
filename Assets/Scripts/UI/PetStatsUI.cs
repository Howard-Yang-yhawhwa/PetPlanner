using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.UI;

public class PetStatsUI : MonoBehaviour
{
    [SerializeField] GameObject DisplayObject;
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text LevelText;
    [SerializeField] Slider HealthBar;
    [SerializeField] TMP_Text HealthText;
    [SerializeField] Slider HungerBar;
    [SerializeField] TMP_Text HungerText;
    [SerializeField] Slider HappinessBar;
    [SerializeField] TMP_Text HappinessText;
    [SerializeField] Slider ExperienceBar;
    [SerializeField] TMP_Text ExperienceText;
    [SerializeField] Image DragonIconImage;
    [SerializeField] RawImage AnimatedDragonIconImage;

    Subscription<PetSelectedEvent> selected_event;
    Subscription<PetStatsUpdateEvent> stats_update_event;
    Subscription<OwnedPetsUpdateEvent> petlist_update_event;

    string currDisplayID;

    private void Awake()
    {
        selected_event = EventBus.Subscribe<PetSelectedEvent>(OnPetSelection);
        stats_update_event = EventBus.Subscribe<PetStatsUpdateEvent>(OnStatsUpdateEvent);
        petlist_update_event = EventBus.Subscribe<OwnedPetsUpdateEvent>(OnPetListUpate);

        DisplayObject.SetActive(false);
    }

    void OnPetListUpate(OwnedPetsUpdateEvent e)
    {
        UpdateDisplay();
    }

    void OnStatsUpdateEvent(PetStatsUpdateEvent e)
    {
        if (e.petID == currDisplayID)
        {
            UpdateDisplay();
        }
    }

    void OnPetSelection(PetSelectedEvent e)
    {
        currDisplayID = e.petID;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (currDisplayID == null || !Player.OwnedPets.ContainsKey(currDisplayID))
        {
            return;
        }

        PetData data = Player.OwnedPets[currDisplayID];
        PetDataSO dataSO = PetManager.AvaliableSOs[data.Type];

        NameText.text = data.Nickname;
        LevelText.text = $"{data.Level}";
        HealthText.text = $"{data.CurrentHealth} / {dataSO.MaxHealth}";
        HungerText.text = $"{data.CurrentHunger} / {dataSO.MaxHunger}";
        HappinessText.text = $"{data.CurrentHappiness} / {dataSO.MaxHappiness}";
        ExperienceText.text = $"{data.CurrentExperience} / {PetManager.GetMaxExp(data.Level)}";
        DragonIconImage.sprite = dataSO.DisplayIcon;
        AnimatedDragonIconImage.texture = dataSO.AnimatedIcon;
        // AnimatedIconsManager.Instance.DisplayAnimatedIcon(dataSO.Type);

        float healthPercent = data.CurrentHealth / dataSO.MaxHealth;
        float hungerPercent = data.CurrentHunger / dataSO.MaxHunger;
        float happinessPercent = data.CurrentHappiness / dataSO.MaxHappiness;
        float xpPercent = data.CurrentExperience / PetManager.GetMaxExp(data.Level);

        HealthBar.value = healthPercent;
        HungerBar.value = hungerPercent;
        HappinessBar.value = happinessPercent;
        ExperienceBar.value = xpPercent;

        DisplayObject.SetActive(true);
    }
}

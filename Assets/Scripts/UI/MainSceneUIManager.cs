using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSceneUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text gemsText;
    [SerializeField] Button backButton;
    [SerializeField] GameObject ShopUI;
    [SerializeField] InventoryUIManager InventoryUI;
    [SerializeField] GameObject TaskUI;
    [SerializeField] GameObject PetSelectUI;

    Subscription<CurrencyUpdateEvent> currency_update_event;

    private void Awake()
    {
        currency_update_event = EventBus.Subscribe<CurrencyUpdateEvent>(OnCurrencyUpdateEvent);
    }

    private void Start()
    {
        ToggleShopUI(false);
        ToggleTaskUI(false);
        ToggleInventoryUI(true);

        backButton.onClick.AddListener(CloseAllUI);

        coinsText.text = $"{Player.Coins}";
        gemsText.text = $"{Player.Gems}";
    }

    private void Update()
    {
        backButton.gameObject.SetActive(TaskUI.activeSelf || ShopUI.activeSelf);
        InventoryUI.gameObject.SetActive(!TaskUI.activeSelf);
    }

    void OnCurrencyUpdateEvent(CurrencyUpdateEvent e)
    {
        coinsText.text = $"{Player.Coins}";
        gemsText.text = $"{Player.Gems}";
    }

    public void CreateDebugTask()
    {
        string ID = RandomUtils.GenerateNumericCode(10);

        Task task = new Task();
        task.title = "Debug Title";
        task.notes = "This task is for debug use only";
        task.priority = Priority.VeryHigh;
        task.etdUnits = TimeUnits.Day;
        task.etdValue = 1.2f;
        task.ID = ID;

        TasksManager.AddTask(task.ID, task);
    }

    public void CloseAllUI()
    {
        ShopUI.SetActive(false);
        TaskUI.SetActive(false);
        InventoryUI.ShowBackground(false);

        EventBus.Publish(new ChangeActionMapEvent(PlayerActionMaps.Gameplay));
    }

    public void TogglePetSelectUI(bool status)
    {
        PetSelectUI.SetActive(status);

        EventBus.Publish(new ChangeActionMapEvent(status ? PlayerActionMaps.UI : PlayerActionMaps.Gameplay));
    }

    public void ToggleShopUI(bool status)
    {
        ShopUI.SetActive(status);
        InventoryUI.ShowBackground(status);
        EventBus.Publish(new ChangeActionMapEvent(status ? PlayerActionMaps.UI : PlayerActionMaps.Gameplay));
    }

    public void ToggleTaskUI(bool status)
    {
        TaskUI.SetActive(status);
        EventBus.Publish(new ChangeActionMapEvent(status ? PlayerActionMaps.UI : PlayerActionMaps.Gameplay));
    }

    public void ToggleInventoryUI(bool status)
    {
        InventoryUI.gameObject.SetActive(status);
    }
}

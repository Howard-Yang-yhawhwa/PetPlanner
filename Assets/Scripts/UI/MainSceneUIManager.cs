using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSceneUIManager : MonoBehaviour
{
    [SerializeField] TMP_Text currecyText;
    [SerializeField] Button backButton;
    [SerializeField] GameObject ShopUI;
    [SerializeField] GameObject InventoryUI;
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

        currecyText.text = $"{Player.Currency}";
    }

    private void Update()
    {
        backButton.gameObject.SetActive(TaskUI.activeSelf || ShopUI.activeSelf);
        InventoryUI.SetActive(!TaskUI.activeSelf);
    }

    void OnCurrencyUpdateEvent(CurrencyUpdateEvent e)
    {
        currecyText.text = $"{e.newValue}";
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
        //InventoryUI.SetActive(false);
        TaskUI.SetActive(false);
    }

    public void TogglePetSelectUI(bool status)
    {
        PetSelectUI.SetActive(status);
    }

    public void ToggleShopUI(bool status)
    {
        ShopUI.SetActive(status);
    }

    public void ToggleTaskUI(bool status)
    {
        TaskUI.SetActive(status);
    }

    public void ToggleInventoryUI(bool status)
    {
        InventoryUI.SetActive(status);
    }
}

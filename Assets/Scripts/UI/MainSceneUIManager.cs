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
    [SerializeField] GameObject[] bottomDisplays;
    [SerializeField] TabbedWindowInteractionHandler bottomTabHandler;
    [SerializeField] FullPetInfoDisplay petInfoDisplay;
    

    [Header("=== DEBUG INFO ===")]
    [SerializeField] int coinsVal;
    [SerializeField] int gemsVal;

    Subscription<CurrencyUpdateEvent> currency_update_event;
    Subscription<CloseAllUIEvent> close_all_ui_event;
    Subscription<DisplayBottomBarEvent> close_bottom_bar_event;

    private void Awake()
    {
        currency_update_event = EventBus.Subscribe<CurrencyUpdateEvent>(OnCurrencyUpdateEvent);
        close_all_ui_event = EventBus.Subscribe<CloseAllUIEvent>(e => CloseAllUI());
        close_bottom_bar_event = EventBus.Subscribe<DisplayBottomBarEvent>(OnDisplayBottomBarEvent);
    }

    private void Start()
    {
        ToggleShopUI(false);
        ToggleTaskUI(false);
        // ToggleInventoryUI(true);

        backButton.onClick.AddListener(CloseAllUI);

        coinsVal = Player.Coins;
        gemsVal = Player.Gems;

        coinsText.text = coinsVal.ToString();
        gemsText.text = gemsVal.ToString();

    }

    private void Update()
    {
        backButton.gameObject.SetActive(TaskUI.activeSelf || ShopUI.activeSelf);
        // InventoryUI.gameObject.SetActive(!TaskUI.activeSelf);
    }

    void OnCurrencyUpdateEvent(CurrencyUpdateEvent e)
    {
        coinsVal = Player.Coins;
        gemsVal = Player.Gems;

        coinsText.text = coinsVal.ToString();
        gemsText.text = gemsVal.ToString();
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

    void OnDisplayBottomBarEvent(DisplayBottomBarEvent e)
    {
        DisplayBottomBar(e.shouldDisplay);
    }

    public void DisplayBottomBar(bool shouldDisplay)
    {
        foreach (GameObject display in bottomDisplays)
        {
            display.SetActive(shouldDisplay);
        }

        if(!shouldDisplay) petInfoDisplay.Close();
    }

    public void CloseAllUI()
    {
        if (ShopUI.activeSelf) ToggleShopUI(false);
        if (TaskUI.activeSelf) ToggleTaskUI(false);

        petInfoDisplay.Close();

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

        foreach(GameObject display in bottomDisplays)
        {
            display.SetActive(true);
        }

        bottomTabHandler.SwitchTabByIndex(status ? 1 : 0);

        if (status) petInfoDisplay.Close();

        EventBus.Publish(new ChangeActionMapEvent(status ? PlayerActionMaps.UI : PlayerActionMaps.Gameplay));
    }

    public void ToggleTaskUI(bool status)
    {
        TaskUI.SetActive(status);

        foreach (GameObject display in bottomDisplays)
        {
            display.SetActive(!status);
        }

        if (status) petInfoDisplay.Close();

        EventBus.Publish(new ChangeActionMapEvent(status ? PlayerActionMaps.UI : PlayerActionMaps.Gameplay));
    }

}

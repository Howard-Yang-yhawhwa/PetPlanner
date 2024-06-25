using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] InventoryButton buttonPrefab;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image fillImage;
    [SerializeField] GameObject titleDisplay;

    Subscription<InventoryUpdateEvent> update_event;
    Dictionary<ShopItemTypes, InventoryButton> inventoryButtonMap = new Dictionary<ShopItemTypes, InventoryButton>();

    private void Awake()
    {
        update_event = EventBus.Subscribe<InventoryUpdateEvent>(OnUpdateEvent);
    }

    public void ShowBackground(bool status)
    {
        Color targetColor = backgroundImage.color;
        targetColor.a = status ? 1 : 0;
        backgroundImage.color = targetColor;

        targetColor = fillImage.color;
        targetColor.a = status ? 1 : 0;
        fillImage.color = targetColor;

        titleDisplay.SetActive(status);
    }

    private void Start()
    {
        foreach(DefaultShopItemSO itemSO in ShopManager.Instance.AvaliableShopItems)
        {
            InventoryButton clone = Instantiate(buttonPrefab, container);
            clone.Setup(itemSO.type);
            inventoryButtonMap.Add(itemSO.type, clone);
        }

        foreach (DefaultShopItemSO item in ShopManager.Instance.AvaliableShopItems)
        {
            if (!Player.Inventory.ContainsKey(item.type))
            {
                Player.Inventory.Add(item.type, 0);
            }
        }

        UpdateDisplay();
    }

    void OnUpdateEvent(InventoryUpdateEvent e)
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {

        foreach (var kvp in Player.Inventory)
        {
            ShopItemTypes type = kvp.Key;
            int amount = kvp.Value;
            inventoryButtonMap[type].gameObject.SetActive(amount != 0);
            inventoryButtonMap[type].UpdateAmount(amount);
        }
    }
}

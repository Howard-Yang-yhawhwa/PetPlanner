using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] InventoryButton buttonPrefab;

    Subscription<InventoryUpdateEvent> update_event;

    private void Awake()
    {
        update_event = EventBus.Subscribe<InventoryUpdateEvent>(OnUpdateEvent);
    }

    private void Start()
    {
        foreach(var kvp in Player.Inventory)
        {
            ShopItemTypes type = kvp.Key;
            int amount = kvp.Value;

            InventoryButton clone = Instantiate(buttonPrefab, container);
            clone.Setup(type);
        }
    }

    void OnUpdateEvent(InventoryUpdateEvent e)
    {
        Debug.Log($"OnUpdateEvent Called in [InventoryUIManager]");

        foreach(Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (var kvp in Player.Inventory)
        {
            ShopItemTypes type = kvp.Key;
            int amount = kvp.Value;

            InventoryButton clone = Instantiate(buttonPrefab, container);
            clone.Setup(type);

        }
    }
}

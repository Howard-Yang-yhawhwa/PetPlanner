using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{

    [Header("Default Shop Stuff")]
    [SerializeField] Transform DefaultShopContainer;
    [SerializeField] GameObject AnchorObject;
    [SerializeField] ShopItemUI ShopItemPrefab;


    private void Start()
    {
        for(int i = ShopManager.Instance.AvaliableShopItems.Count - 1; i >= 0; --i)
        {
            DefaultShopItemSO itemData = ShopManager.Instance.AvaliableShopItems[i];
            ShopItemUI clone = Instantiate(ShopItemPrefab, DefaultShopContainer);
            clone.Setup(itemData);
            clone.transform.SetSiblingIndex(AnchorObject.transform.GetSiblingIndex() + 1);
        }
    }
}

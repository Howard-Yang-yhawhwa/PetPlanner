using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image iconImage;
    [SerializeField] Image iconBackgroundImage;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text costText;

    DefaultShopItemSO data;

    public void Setup(DefaultShopItemSO itemData)
    {
        data = itemData;

        iconImage.sprite = data.icon;
        iconBackgroundImage.color = data.iconBackgroundColor;
        nameText.text = data.displayName;

        string descriptionStr = "";

        foreach (ShopItemEffect effect in data.itemEffects)
        {
            descriptionStr += effect.statsType.ToString() + " " + (effect.isPercentage ? $"+ {effect.value * 100}%\n" : $"+ {effect.value}\n");
        }

        descriptionText.text = descriptionStr;

        costText.text = $"$ {itemData.cost}";
    }

    public void TryBuyItem()
    {
        if (Player.Currency >= data.cost)
        {
            Player.Currency -= data.cost;
            Player.AddItem(data.type);
        }
    }
}

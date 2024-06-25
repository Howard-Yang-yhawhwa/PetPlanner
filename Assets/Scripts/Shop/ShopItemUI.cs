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
    [SerializeField] TMP_Text[] costTexts; // 0 = Real Money, 1 = Coins, 2 = Gems
    [SerializeField] GameObject[] CurrencyTypeDisplays; // 0 = Real Money, 1 = Coins, 2 = Gems

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
            if (effect.statsType == PetStats.Others)
            {
                descriptionStr += effect.overrideDesription + "\n";
            }
            else
            {
                descriptionStr += effect.statsType.ToString() + " " + (effect.isPercentage ? $"+ {effect.value * 100}%\n" : $"+ {effect.value}\n");
            }
        }

        descriptionText.text = descriptionStr;

        costTexts[(int)itemData.currecyType].text = itemData.currecyType == CurrecyTypes.RealMoney ? "$" : "" + $"{itemData.cost}";

        foreach(GameObject display in CurrencyTypeDisplays)
        {
            display.SetActive(false);
        }

        CurrencyTypeDisplays[(int)itemData.currecyType].SetActive(true);
    }

    public void TryBuyItem()
    {
        bool successful = false;
        switch (data.currecyType) 
        { 
            case CurrecyTypes.Coins:
                if (Player.Coins >= data.cost)
                {
                    Player.Coins -= data.cost;
                    successful = true;
                }
                break;
            case CurrecyTypes.Gems:
                if (Player.Gems >= data.cost)
                {
                    Player.Gems -= data.cost;
                    successful = true;
                }
                break;
        }

        if (successful) Player.AddItem(data.type);
    }
}

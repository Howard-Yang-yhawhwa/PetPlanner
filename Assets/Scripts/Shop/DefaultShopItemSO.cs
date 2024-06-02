using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item Data", menuName = "ScriptableObjects/Default Shop Item")]
public class DefaultShopItemSO : ScriptableObject
{
    [Header("=== Basic Info ==")]
    public ShopItemTypes type;
    public Sprite icon;
    public Color iconBackgroundColor;
    public string displayName;

    [Space(20)]
    [Header(" === Content ===")]
    public List<ShopItemEffect> itemEffects;
    public int cost;
    public CurrecyTypes currecyType = CurrecyTypes.Coins;
    public int amount = 1;
}

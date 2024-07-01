using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopItemTypes { UniversalPetFood, UniversalPetTreat, UniversalPetToy, CommonMedicine, Coins, Gems, Revival }
public enum CurrecyTypes { RealMoney, Coins, Gems }

[System.Serializable]
public class ShopItemEffect
{
    public PetStats statsType;
    public bool isPercentage;
    public float value;
    [TextArea] public string overrideDesription;
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] public List<DefaultShopItemSO> AvaliableShopItems;

    [Header("=== Global Visuals ===")]
    public Sprite coinsIcon;
    public Sprite gemsIcon;

    public static Dictionary<ShopItemTypes, DefaultShopItemSO> shopItemsMap = new Dictionary<ShopItemTypes, DefaultShopItemSO>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach(DefaultShopItemSO itemSO in AvaliableShopItems)
        {
            shopItemsMap.Add(itemSO.type, itemSO);
        }
    }
}

using Ricimi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GiftRecievedDisplay : MonoBehaviour
{
    [SerializeField] float giftSpawnDisplay = 0.1f;

    [Header("=== References ===")]
    [SerializeField] GameObject resultDisplay;
    [SerializeField] GameObject giftDisplay;
    [SerializeField] Popup popupManager;

    [Header("--- Result Displays ---")]
    [SerializeField] ParticleSystem confettiVFX;
    [SerializeField] GiftItemContent giftItemContentPrefab;
    [SerializeField] Transform giftItemContainer;

    List<GiftData> currentGiftList = new List<GiftData>();

    private void ResetDisplay()
    {
        for (int i = 1; i < giftItemContainer.childCount; i++)
        {
            Destroy(giftItemContainer.GetChild(i).gameObject);
        }

        giftDisplay.SetActive(true);
        resultDisplay.SetActive(false);
    }

    public void OpenDisplay(List<GiftData> gifts)
    {
        ResetDisplay();
        currentGiftList = gifts;
        popupManager.Open();
    }

    public void ShowResult()
    {
        confettiVFX.Play();
        giftDisplay.SetActive(false);
        resultDisplay.SetActive(true);

        StartCoroutine(ShowResultRoutine());
    }

    public IEnumerator ShowResultRoutine()
    {
        foreach (GiftData giftData in currentGiftList)
        {
            GiftItemContent clone = Instantiate(giftItemContentPrefab, giftItemContainer);
            Sprite displayIcon = null;
            string displayValue = "";

            if (giftData.type == ShopItemTypes.Coins)
            {
                displayIcon = ShopManager.Instance.coinsIcon;
            }
            else if (giftData.type == ShopItemTypes.Gems)
            {
                displayIcon = ShopManager.Instance.gemsIcon;
            }
            else
            {
                DefaultShopItemSO dataSO = ShopManager.shopItemsMap[giftData.type];
                displayIcon = dataSO.icon;
            }

            displayValue = giftData.amount.ToString();
            clone.SetContent(displayValue, displayIcon);

            yield return new WaitForSeconds(giftSpawnDisplay);
        }
    }
    
    public void CloseDisplay()
    {
        popupManager.Close();
    }
}

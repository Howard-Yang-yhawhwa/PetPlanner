using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvlogContent : MonoBehaviour
{
    [SerializeField] TMP_Text logText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] Button giftButton;

    List<GiftData> giftList;

    public void SetContent(string log, string time, List<GiftData> giftList)
    {
        logText.text = log;
        timeText.text = time;
        this.giftList = giftList;
        giftButton.gameObject.SetActive(giftList.Count > 0);
    }

    public void OnGiftButtonClicked()
    {
        Debug.Log("Gift button clicked!");
        EventBus.Publish(new OpenGiftRecievedPopupEvent(giftList));
    }

}

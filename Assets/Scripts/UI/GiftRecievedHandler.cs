using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftRecievedHandler : MonoBehaviour
{
    [SerializeField] GiftRecievedDisplay giftRecievedDisplay;

    Subscription<OpenGiftRecievedPopupEvent> open_display_event;

    private void Awake()
    {
        open_display_event = EventBus.Subscribe<OpenGiftRecievedPopupEvent>(OnOpenGiftRecievedPopup);
    }

    void OnOpenGiftRecievedPopup(OpenGiftRecievedPopupEvent e)
    {
        giftRecievedDisplay.OpenDisplay(e.gifts);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftItemContent : MonoBehaviour
{
    [SerializeField] TMP_Text valueText;
    [SerializeField] Image iconImage;

    public void SetContent(string value, Sprite icon)
    {
        valueText.text = value;
        iconImage.sprite = icon;
    }
}

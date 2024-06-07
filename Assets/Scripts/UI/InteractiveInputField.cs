using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractiveInputField : MonoBehaviour
{
    [SerializeField] TMP_InputField myInputfield;
    [SerializeField] TMP_Text myInputText;
    [SerializeField] TMP_Text myText;

    private void Awake()
    {
        SyncTextfield();
    }

    public void OnValueChanged()
    {
        SyncTextfield();
    }

    void SyncTextfield()
    {
        
        int lineCount = myInputText.textInfo.lineCount;
        TMP_LineInfo[] lineInfos = myInputText.textInfo.lineInfo;

        Debug.Log($"Resizing Inputfield... Line Count: {lineCount}");
        
        string syncText = "";

        for (int i = 0; i < lineCount + 2; ++i)
        {
            syncText += "\n";
        }
        
        myText.text = syncText;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabbedWindowInteractionHandler : MonoBehaviour
{
    [SerializeField] Toggle[] tabToggles;
    [SerializeField] GameObject[] tabContents;
    [SerializeField] GameObject moreButtonObject;

    Dictionary<Toggle, int> toggleToIndex = new Dictionary<Toggle, int>();
    int currentIndex;

    private void Awake()
    {
        int tabIndex = 0;
        foreach(Toggle toggle in tabToggles)
        {
            toggleToIndex.Add(toggle, tabIndex);
            toggle.onValueChanged.AddListener(delegate { SwitchTab(toggle); });
            tabIndex += 1;
        }

        SwitchTab(tabToggles[0]);
    }

    public void SwitchTabByIndex(int index)
    {
        tabToggles[index].isOn = true;
    }

    public void SwitchTab(Toggle toggle)
    {
        int tabIndex = toggleToIndex[toggle];

        if (!toggle.isOn || tabContents[tabIndex].activeSelf == true) return;

        currentIndex = tabIndex;

        moreButtonObject.SetActive(tabIndex == 0);

        for (int i = 0; i < tabContents.Length; i++)
        {
            tabContents[i].SetActive(i == tabIndex);
        }
    }
}

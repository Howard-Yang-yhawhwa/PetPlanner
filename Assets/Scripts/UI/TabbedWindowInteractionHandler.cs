using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabbedWindowInteractionHandler : MonoBehaviour
{
    [SerializeField] Toggle[] tabToggles;
    [SerializeField] GameObject[] tabContents;

    Dictionary<Toggle, int> toggleToIndex = new Dictionary<Toggle, int>();

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

    public void SwitchTab(Toggle toggle)
    {
        if (!toggle.isOn) return;
        int tabIndex = toggleToIndex[toggle];
        for (int i = 0; i < tabContents.Length; i++)
        {
            tabContents[i].SetActive(i == tabIndex);
        }
    }
}

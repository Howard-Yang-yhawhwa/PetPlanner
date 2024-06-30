using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullPetInfoDisplay : MonoBehaviour
{
    [SerializeField] GameObject displayObject;
    [SerializeField] Button openButton;
    [SerializeField] Button closeButton;
    [SerializeField] float CloseYPos;
    [SerializeField] float OpenYPos;
    [SerializeField] float AnimationLerpDuration;
    [SerializeField] AnimationCurve AnimationCurve;

    [Header("=== Advlog Display ===")]
    [SerializeField] Transform advlogContainer;
    [SerializeField] AdvlogContent advlogContent;

    [Header("=== DEBUG INFO ===")]
    [SerializeField] bool debugMode;

    Dictionary<string, AdvlogContent> spawnedAdvlogs = new Dictionary<string, AdvlogContent>();
    Subscription<PetStatsUpdateEvent> pet_stats_update_event;
    Subscription<PetSelectedEvent> pet_selected_event;

    private void Awake()
    {
        pet_stats_update_event = EventBus.Subscribe<PetStatsUpdateEvent>(OnPetStatsUpdateEvent);
        pet_selected_event = EventBus.Subscribe<PetSelectedEvent>(OnPetSelectedEvent);
    }

    private void Start()
    {
        // Default to Close Position
        displayObject.SetActive(false);
        ((RectTransform)displayObject.transform).anchoredPosition3D = new Vector3(((RectTransform)displayObject.transform).anchoredPosition3D.x, CloseYPos, ((RectTransform)displayObject.transform).anchoredPosition3D.z);
        
        // Set button events
        openButton.onClick.AddListener(Open);
        closeButton.onClick.AddListener(Close);

        // Debug Code
        if (debugMode)
        {
            displayObject.SetActive(true);
            StartCoroutine(OpenCloseAnimation());
            displayObject.SetActive(false);
        }   
    }

    void OnPetSelectedEvent(PetSelectedEvent e)
    {
        PetData petData = Player.OwnedPets[e.petID];
        UpdateDisplay(petData);
    }

    void OnPetStatsUpdateEvent(PetStatsUpdateEvent e)
    {
        PetData petData = Player.OwnedPets[e.petID];
        UpdateDisplay(petData);
    }

    void UpdateDisplay(PetData petData)
    {
        HashSet<string> avaliableTimestamps = new HashSet<string>();

        // Clean up old advlogs if they are no longer in the pet's data
        foreach (AdvlogData data in petData.myAdvlogs)
        {
            avaliableTimestamps.Add(data.timestamp);
        }

        List<string> removeList = new List<string>();
        foreach (var item in spawnedAdvlogs)
        {
            if (!avaliableTimestamps.Contains(item.Key))
            {
                Destroy(item.Value.gameObject);
                removeList.Add(item.Key);
            }
        }

        foreach(string timestamp in removeList)
        {
            spawnedAdvlogs.Remove(timestamp);
        }

        // Update the advlogs
        foreach (AdvlogData data in petData.myAdvlogs)
        {
            if (!spawnedAdvlogs.ContainsKey(data.timestamp))
            {
                AdvlogContent clone = Instantiate(advlogContent, advlogContainer);
                spawnedAdvlogs.Add(data.timestamp, clone);
            }

            AdvlogDataSO dataSO = AdvlogManager.Instance.NameLookup[data.SOName];
            spawnedAdvlogs[data.timestamp].SetContent(dataSO.log, data.timestamp, dataSO.gifts);
        }
    }

    public void Open()
    {
        displayObject.SetActive(true);
        StartCoroutine(OpenAnimation());
    }

    public void Close()
    {
        StartCoroutine(CloseAnimation());
        
    }

    IEnumerator OpenAnimation()
    {
        float timeElapsed = 0;
        while (timeElapsed <= AnimationLerpDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = AnimationCurve.Evaluate(timeElapsed / AnimationLerpDuration);
            float yPos = Mathf.Lerp(CloseYPos, OpenYPos, t);
            ((RectTransform)displayObject.transform).anchoredPosition3D = new Vector3(((RectTransform)displayObject.transform).anchoredPosition3D.x, yPos, ((RectTransform)displayObject.transform).anchoredPosition3D.z);
            yield return null;
        }
    }

    IEnumerator CloseAnimation()
    {
        float timeElapsed = 0;
        while (timeElapsed <= AnimationLerpDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = AnimationCurve.Evaluate(timeElapsed / AnimationLerpDuration);
            float yPos = Mathf.Lerp(OpenYPos, CloseYPos, t);
            ((RectTransform)displayObject.transform).anchoredPosition3D = new Vector3(((RectTransform)displayObject.transform).anchoredPosition3D.x, yPos, ((RectTransform)displayObject.transform).anchoredPosition3D.z);
            yield return null;
        }

        displayObject.SetActive(false);

    }

    IEnumerator OpenCloseAnimation()
    {
        yield return StartCoroutine(OpenAnimation());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(CloseAnimation());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedIconsManager : MonoBehaviour
{
    public static AnimatedIconsManager Instance;

    [SerializeField] List<AnimatedIconRenderer> iconRenderers = new List<AnimatedIconRenderer>();
    Dictionary<PetTypes, AnimatedIconRenderer> rendererMap = new Dictionary<PetTypes, AnimatedIconRenderer>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        foreach(AnimatedIconRenderer iconRenderer in iconRenderers)
        {
            rendererMap.Add(iconRenderer.type, iconRenderer);
        }

        HideAllAnimatedIcons();
    }

    public void DisplayAnimatedIcon(PetTypes type)
    {
        if (rendererMap.ContainsKey(type))
        {
            HideAllAnimatedIcons();
            rendererMap[type].gameObject.SetActive(true);
        }
    }

    public void HideAllAnimatedIcons()
    {
        foreach (KeyValuePair<PetTypes, AnimatedIconRenderer> entry in rendererMap)
        {
            entry.Value.gameObject.SetActive(false);
        }
    }
}

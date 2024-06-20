using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDrawResultHandler : MonoBehaviour
{

    [SerializeField] EggDrawResultDisplay display;
    Subscription<OpenDrawPetResultEvent> draw_result_event;

    private void Awake()
    {
        draw_result_event = EventBus.Subscribe<OpenDrawPetResultEvent>(OnDrawResultEvent);
    }

    void OnDrawResultEvent(OpenDrawPetResultEvent e)
    {
        display.InitAndOpen(e.petID);
    }
}

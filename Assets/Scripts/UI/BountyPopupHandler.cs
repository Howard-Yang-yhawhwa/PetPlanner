using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyPopupHandler : MonoBehaviour
{
    [SerializeField] BountyEarnedPopup bountyEarnedPopup;
    Subscription<AddBountyEvent> add_bounty_event;

    private void Awake()
    {
        add_bounty_event = EventBus.Subscribe<AddBountyEvent>(OnAddBountyEvent);
    }

    void OnAddBountyEvent(AddBountyEvent e)
    {
        bountyEarnedPopup.InitAndOpen(e.amount);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDragPassThrough : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("=== Main Settings ===")]
    [SerializeField] float horizontalDragMinDist = 30f;
    [SerializeField] float verticalDragMinDist = 8f;


    [Header("=== DEBUG INFO ===")]
    [SerializeField] Vector3 mouseStartPos;
    [SerializeField] int originalLeftPadding;
    [SerializeField] int originalRightPadding;
    [SerializeField] bool horizontalDrag = false;
    [SerializeField] bool verticalDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseStartPos = Input.mousePosition;

        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 distance = Input.mousePosition - mouseStartPos;

        if (!horizontalDrag && !verticalDrag)
        {
            if (Mathf.Abs(distance.x) > horizontalDragMinDist)
            {
                horizontalDrag = true;
                return;
            }
            else if (Mathf.Abs(distance.y) > verticalDragMinDist)
            {
                verticalDrag = true;
                return;
            }
        }

        if (verticalDrag)
        {
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        horizontalDrag = false;
        verticalDrag = false;

        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
    }
}

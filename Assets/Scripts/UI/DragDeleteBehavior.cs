using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class DragDeleteBehavior : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("=== Main Settings ===")]
    [SerializeField] VerticalLayoutGroup targetLayoutGroup;
    [SerializeField] float deleteThreshold;
    [SerializeField] float horizontalDragMinDist = 30f;
    [SerializeField] float verticalDragMinDist = 8f;

    [Header("=== Actions ===")]
    [SerializeField] UnityEvent OnDeleteAction;

    [Header("=== DEBUG INFO ===")]
    [SerializeField] Vector3 mouseStartPos;
    [SerializeField] int originalLeftPadding;
    [SerializeField] int originalRightPadding;
    [SerializeField] bool horizontalDrag = false;
    [SerializeField] bool verticalDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseStartPos = Input.mousePosition;
        originalLeftPadding = targetLayoutGroup.padding.left;
        originalRightPadding = targetLayoutGroup.padding.right;

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
            } else if (Mathf.Abs(distance.y) > verticalDragMinDist)
            {
                verticalDrag = true;
                return;
            }
        }

        if (horizontalDrag && distance.x < 0)
        {
            RectOffset temp = new RectOffset(targetLayoutGroup.padding.left, targetLayoutGroup.padding.right,
                targetLayoutGroup.padding.top, targetLayoutGroup.padding.bottom);
            temp.left = Mathf.RoundToInt(originalLeftPadding + distance.x);
            temp.right = Mathf.RoundToInt(originalRightPadding - distance.x);
            targetLayoutGroup.padding = temp;
        }
        else if (verticalDrag)
        {
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (horizontalDrag)
        {
            Vector3 distance = Input.mousePosition - mouseStartPos;

            if (-distance.x > deleteThreshold)
            {
                OnDeleteAction.Invoke();
            }
            else
            {
                RectOffset temp = new RectOffset(targetLayoutGroup.padding.left, targetLayoutGroup.padding.right,
                targetLayoutGroup.padding.top, targetLayoutGroup.padding.bottom);
                temp.left = originalLeftPadding;
                temp.right = originalRightPadding;
                targetLayoutGroup.padding = temp;
            }
        }

        horizontalDrag = false;
        verticalDrag = false;

        ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
    }
}

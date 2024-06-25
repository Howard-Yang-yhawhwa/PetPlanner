using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerActionMaps { UI, Gameplay }
public class FreelookCameraTouchInput : MonoBehaviour
{
    [SerializeField] float XAxisSensitivity = 10f;
    [SerializeField] float YAxisSensitivity = 10f;

    PlayerActionMaps currentActionMap;

    Subscription<ChangeActionMapEvent> change_action_map_event;

    private void Awake()
    {
        change_action_map_event = EventBus.Subscribe<ChangeActionMapEvent>(OnActionMapChanged);
    }

    void OnActionMapChanged(ChangeActionMapEvent e)
    {
        currentActionMap = e.newMap;
    }

    void Start()
    {
        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
    }

    float HandleAxisInputDelegate(string axisName)
    {
        // Stop camera control if player is on UI
        if (currentActionMap != PlayerActionMaps.Gameplay)
        {
            return 0f;
        }

        switch (axisName)
        {

            case "Mouse X":

                if (Input.touchCount > 0)
                {
                    return Input.touches[0].deltaPosition.x / XAxisSensitivity;
                }
                else
                {
                    return Input.GetAxis(axisName);
                }

            case "Mouse Y":
                if (Input.touchCount > 0)
                {
                    return Input.touches[0].deltaPosition.y / YAxisSensitivity;
                }
                else
                {
                    return Input.GetAxis(axisName);
                }

            default:
                Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
                break;
        }

        return 0f;
    }
}

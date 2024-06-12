using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreelookCameraTouchInput : MonoBehaviour
{
    [SerializeField] float XAxisSensitivity = 10f;
    [SerializeField] float YAxisSensitivity = 10f;

    void Start()
    {
        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
    }

    float HandleAxisInputDelegate(string axisName)
    {
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

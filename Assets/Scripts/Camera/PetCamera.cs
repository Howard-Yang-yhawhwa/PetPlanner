using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PetCamera : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook petCamera;
    [SerializeField] float lerpPeriod = 1f;
    [SerializeField] AnimationCurve lerpCurve;
    [SerializeField] float[] defaultHeights;
    [SerializeField] float[] defaultRadii;

    Subscription<SetCameraScaleEvent> set_camera_scale_event;


    private void Awake()
    {
        set_camera_scale_event = EventBus.Subscribe<SetCameraScaleEvent>(OnSetCameraScaleEvent);
    }

    void OnSetCameraScaleEvent(SetCameraScaleEvent e)
    {
        StartCoroutine(ChangeScaleRoutine(e.factor));
    }

    IEnumerator ChangeScaleRoutine(float factor)
    {
        float timer = 0;
        float[] startHeights = new float[3];
        float[] startRadii = new float[3];
        float[] endHeights = new float[3];
        float[] endRadii = new float[3];

        startHeights[0] = petCamera.m_Orbits[0].m_Height;
        startHeights[1] = petCamera.m_Orbits[1].m_Height;
        startHeights[2] = petCamera.m_Orbits[2].m_Height;
        startRadii[0] = petCamera.m_Orbits[0].m_Radius;
        startRadii[1] = petCamera.m_Orbits[1].m_Radius;
        startRadii[2] = petCamera.m_Orbits[2].m_Radius;

        endHeights[0] = defaultHeights[0] * factor;
        endHeights[1] = defaultHeights[1] * factor;
        endHeights[2] = defaultHeights[2] * factor;
        endRadii[0] = defaultRadii[0] * factor;
        endRadii[1] = defaultRadii[1] * factor;
        endRadii[2] = defaultRadii[2] * factor;

        while (timer < lerpPeriod)
        {
            timer += Time.deltaTime;
            petCamera.m_Orbits[0].m_Height = Mathf.Lerp(startHeights[0], endHeights[0], lerpCurve.Evaluate(timer / lerpPeriod));
            petCamera.m_Orbits[1].m_Height = Mathf.Lerp(startHeights[1], endHeights[1], lerpCurve.Evaluate(timer / lerpPeriod));
            petCamera.m_Orbits[2].m_Height = Mathf.Lerp(startHeights[2], endHeights[2], lerpCurve.Evaluate(timer / lerpPeriod));
            petCamera.m_Orbits[0].m_Radius = Mathf.Lerp(startRadii[0], endRadii[0], lerpCurve.Evaluate(timer / lerpPeriod));
            petCamera.m_Orbits[1].m_Radius = Mathf.Lerp(startRadii[1], endRadii[1], lerpCurve.Evaluate(timer / lerpPeriod));
            petCamera.m_Orbits[2].m_Radius = Mathf.Lerp(startRadii[2], endRadii[2], lerpCurve.Evaluate(timer / lerpPeriod));
            yield return null;
        }
    }
}

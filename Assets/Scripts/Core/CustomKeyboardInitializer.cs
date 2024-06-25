using System.Collections;
using System.Collections.Generic;
using UMI;
using UnityEngine;

public class CustomKeyboardInitializer : MonoBehaviour
{
    void Awake()
    {
        MobileInput.Init();
    }
}

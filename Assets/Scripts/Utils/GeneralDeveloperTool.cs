using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDeveloperTool : MonoBehaviour
{
    [SerializeField] int deltaCurrency;
    public void AddCurrency()
    {
        Player.Currency += deltaCurrency;
    }
}

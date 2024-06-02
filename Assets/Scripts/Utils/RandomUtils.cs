using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUtils : MonoBehaviour
{
    public static string GenerateNumericCode(int length)
    {
        string code = "";
        char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        for (int i = 0; i < length; ++i)
        {
            code += digits[Random.Range(0, digits.Length)];
        }

        return code;
    }
}

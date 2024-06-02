using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TimeUnits { Week = 4, Day = 3, Hour = 2, Minute = 1, Second = 0 }

public class TimeUtils : MonoBehaviour
{

    public static float ConvertTime(TimeUnits fromUnit, float value, TimeUnits toUnit)
    {
        // Convert everything to seconds
        float seconds = -1;
        switch (fromUnit)
        {
            case TimeUnits.Week:
                seconds = value * 604800;
                break;
            case TimeUnits.Day:
                seconds = value * 86400;
                break;
            case TimeUnits.Hour:
                seconds = value * 3600;
                break;
            case TimeUnits.Minute:
                seconds = value * 60;
                break;
            default:
                seconds = value;
                break;
        }

        Debug.Log($"Converted {value}({fromUnit}) to {seconds}(s)");

        // Convert back to 
        float rtnVal = -1;
        switch (toUnit)
        {
            case TimeUnits.Week:
                rtnVal = seconds / 604800;
                break;
            case TimeUnits.Day:
                rtnVal = seconds / 86400;
                break;
            case TimeUnits.Hour:
                rtnVal = seconds / 3600;
                break;
            case TimeUnits.Minute:
                rtnVal = seconds / 60;
                break;
            default:
                rtnVal = seconds;
                break;
        }

        Debug.Log($"Converted {seconds}(s) to {rtnVal}({toUnit})");

        return rtnVal;
    }

    public static long GetTimestamp()
    {
        return ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
    }

    public static bool isNighTime()
    {
        var currentTime = DateTime.Now.TimeOfDay;
        TimeSpan nightStart = new TimeSpan(18, 0, 0);
        TimeSpan nightEnd = new TimeSpan(6, 0, 0);

        return currentTime >= nightStart || currentTime < nightEnd;
    }
}

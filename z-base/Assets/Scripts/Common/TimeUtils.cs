using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class TimeUtils
{
    public static bool IsNextDay(long lastTimeOnline, long currentTime)
    {
        DateTime lastTime = lastTimeOnline.NextMidNight();
        long lastTimeSecond = lastTime.TotalSecondTimeStamp();
        long rangeTime = currentTime - lastTimeSecond;
        
        //Log.Info("last time online: " + lastTimeOnline.ToDate() + ", current time: " + currentTime.ToDate());
        if (rangeTime < 0) return false;
        return true;
    }
    public static IEnumerator TimeCountDown(Text timeTxt, long currentTime, Action callBack = null)
    {
        long deadTime = currentTime.NextMidNightTimeStamp();
        long temp = deadTime - currentTime;
        
        while (temp > 0)
        {
            yield return new WaitForSeconds(1);
            temp--;
            timeTxt.text = ToTimeSpanString(temp);
        }
        
        callBack?.Invoke();
    }
    
    public static long GetDayCount(long lastTimeOnline, long currentTime)
    {
        DateTime lastTime = lastTimeOnline.NextMidNight();
        long lastTimeSecond = lastTime.TotalSecondTimeStamp();
        long rangeTime = currentTime - lastTimeSecond;
        rangeTime = rangeTime < 0 ? rangeTime : rangeTime / GetTimeADay();
        return rangeTime + 1;
    }
    
    
    public static long GetGachaTime(long lastTimeGacha, long timeConfig, long currentTime)
    {
        var endTime = lastTimeGacha + timeConfig;
        long rangeTime = currentTime - endTime;
        return rangeTime;
    }
    
    public static IEnumerator TimeGachaCoundown(Text timeTxt, long rangeTime, Action callBack = null)
    {
        while (rangeTime > 0)
        {
            yield return new WaitForSeconds(1);
            rangeTime--;
            timeTxt.text = ToTimeSpanString(rangeTime);
        }
        
        callBack?.Invoke();
    }
    public static long GetTimeADay()
    {
        return 86400;
    }
    
    public static long TotalSecondTimeStamp(this DateTime dateTime)
    {
        return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static DateTime ToDate(this long secondTimeStamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return origin.AddSeconds(secondTimeStamp);
    }

    public static DateTime NextMidNight(this long secondTimeStamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return origin.AddSeconds(secondTimeStamp).Date.AddDays(1).AddSeconds(-1);
    }

    public static long NextMidNightTimeStamp(this long secondTimeStamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return origin.AddSeconds(secondTimeStamp).Date.AddDays(1).AddSeconds(-1).TotalSecondTimeStamp();
    }

    public static string ToTimeSpanString(this TimeSpan timeSpan)
    {
        return timeSpan.ToString(@"hh\:mm\:ss");
    }

    public static string ToTimeSpanString(this long timeSpan)
    {
        var timeSpanConverted = TimeSpan.FromSeconds(timeSpan);
        if (timeSpanConverted.Days > 0)
        {
            return string.Format("{0} {1} {2}", timeSpanConverted.Days, "day",
                timeSpanConverted.ToString(@"hh\:mm\:ss"));
        }
        else
        {
            return timeSpanConverted.ToString(@"hh\:mm\:ss");
        }
    }


    public static string ToTimeSpanStringFull(this long timeSpan)
    {
        var timeSpanConverted = TimeSpan.FromSeconds(timeSpan);
        return timeSpanConverted.ToString(@"d\d\:hh\h\:mm\m\:ss\s");
    }

    public static long TotalDays(this DateTime dateTime)
    {
        return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalDays;
    }


    public static (int, int) CalculateTimeBySecond(int totalSecond)
    {
        var timeCalculated = (minute: totalSecond / 60, second: totalSecond % 60);
        return timeCalculated;
    }

    public static string WithColorTag(this string origin, string colorHex)
    {
        return $"<color=#{colorHex}>{origin}</color>";
    }
}
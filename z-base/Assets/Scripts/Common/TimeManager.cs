using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Ins = null;
    public DateTime currentTime;

    public bool check;

    private Coroutine startTime = null;
    
    private PlayerTime timeData = null;
    //make sure there is only one instance of this always.
    void Awake()
    {
        if (Ins == null)
        {
             Ins = this;
             DontDestroyOnLoad(Ins);
        }
        else if (Ins != this)
        {
            Destroy(gameObject);
        }
        
        UpdateTime();
        timeData = DataPlayer.GetModule<PlayerTime>();
    }

    public void UpdateTime(Action callBack = null)
    {
        CoroutineManager.instance.StartCoroutine(GetTime(SaveTime, callBack));
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (currentTime.TotalSecondTimeStamp() > 1000 && check)
            {
                timeData.SetLastTimeOnline(currentTime.TotalSecondTimeStamp());
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (currentTime.TotalSecondTimeStamp() > 1000 && check)
        {
            timeData.SetLastTimeOnline(currentTime.TotalSecondTimeStamp());
        }
    }

    public void SaveTime(bool check = true)
    {
        if (currentTime.TotalSecondTimeStamp() > 1000 && check)
        {
            timeData.SetLastTimeOnline(currentTime.TotalSecondTimeStamp());
            if (startTime == null)
                startTime = CoroutineManager.instance.StartCoroutine(TimeCountDown());
        }
    }

    public IEnumerator GetTime(Action<bool> callBack = null, Action callBack2 = null)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.microsoft.com");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            //Log.Info("TIME" + www.error);
            check = false;
        }
        else
        {
            if (www.isHttpError)
            {
                www = UnityWebRequest.Get("https://www.google.com");
                yield return www.SendWebRequest();
                if (www.isHttpError)
                {
                    // Log.Info(www.error);
                    check = false;
                }
                else
                {
                    string date = www.GetResponseHeader("date");
                    yield return new WaitForSecondsRealtime(0.1f);
                    currentTime = DateTime.ParseExact(date,
                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AssumeUniversal);
                    check = true;
                }
            }
            else
            {
                string date = www.GetResponseHeader("date");
                yield return new WaitForSecondsRealtime(0.1f);
                currentTime = DateTime.ParseExact(date,
                    "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                    CultureInfo.InvariantCulture.DateTimeFormat,
                    DateTimeStyles.AssumeUniversal);
                check = true;
            }
        }

        callBack?.Invoke(check);
        callBack2?.Invoke();
    }

    public void Add1Day()
    {
        timeData.Add1Day();
      //  Debug.Log("SceneManager.GetActiveScene().name:" +SceneManager.GetActiveScene().name);
      //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator TimeCountDown()
    {
        var aSecond = 1;
        while (true)
        {
            yield return new WaitForSeconds(aSecond);
            currentTime = currentTime.Add(TimeSpan.FromSeconds(1));
        }
    }
}
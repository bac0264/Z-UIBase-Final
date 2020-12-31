using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
             var app = FirebaseApp.DefaultInstance;
             FirebaseAnalytics.LogEvent("run");
        });
    }

    

    public void OnClickButton()
    {
        Debug.Log("on_click");
        FirebaseAnalytics.LogEvent("on_click");
    }

}
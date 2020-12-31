using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ClassListener
{
    LoginListenerData = 0,
    CampaignListenerData = 1,
    WatchAdsListenerData = 2,
    GachaListenerData = 3,
}

public static class PublisherService
{
    private static readonly Dictionary<SubjectType, List<IListener>>
        ResgisteredModules = new Dictionary<SubjectType, List<IListener>>();

    private static void InitModuleConstructor(Type moduleType)
    {
        var firstConstructor = moduleType.GetConstructors()[0];
        object module = null;

        if (!firstConstructor.GetParameters().Any())
        {
            return;
        }

        //Lấy các tham số của constructor
        var constructorParameters = firstConstructor.GetParameters();

        var paramList = new List<object>();
        foreach (var parameter in constructorParameters)
        {
            var _object = (object) 1;
            paramList.Add(_object);
        }

        module = firstConstructor.Invoke(paramList.ToArray());
        Console.Write(module);
    }

    public static void Register()
    {
        ResgisteredModules.Clear();
        foreach (var subjectType in (SubjectType[]) Enum.GetValues(typeof(SubjectType)))
        {
            if (!ResgisteredModules.ContainsKey(subjectType))
                ResgisteredModules.Add(subjectType, new List<IListener>());
        }

        foreach (var listener in (ClassListener[]) Enum.GetValues(typeof(ClassListener)))
        {
            var _class = Type.GetType(listener.ToString());
           // Debug.Log(_class);
            InitModuleConstructor(_class);
        }
    }

    public static void ResgisterModule(SubjectType sub, IListener listener)
    {
        if (!ResgisteredModules.ContainsKey(sub))
        {
            return;
        }

        ResgisteredModules[sub].Add(listener);
    }

    public static void DebugLog()
    {
        int k = 0;
        foreach (var data in ResgisteredModules)
        {
            k++;
            // Debug.Log("---------------");
            // Debug.Log("subject type: " + data.Key);
            for (int i = 0; i < data.Value.Count; i++)
            {
              //  Debug.Log("listener: " + data.Value[i].GetSubjectType());
            }

           // Debug.Log("------------------------");
        }
    }

    public static void NotifyListener(BaseListenerData data)
    {
        if (ResgisteredModules.ContainsKey(data.subjectType))
        {
            var listeners = ResgisteredModules[data.subjectType];
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i].Execute(data);
            }
        }
    }
}
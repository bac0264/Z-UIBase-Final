using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IListener
{
    void Execute(BaseListenerData data);

    SubjectType GetSubjectType();
}

public enum SubjectType
{
    Login = 0,
    PlayCampaign = 1,
    WatchAds = 2,
    Gacha = 3,
}

public class BaseListenerData : IListener
{
    public SubjectType subjectType;

    public int number;
    
    
    // First Constructor to register module in PublisherService
    public BaseListenerData(object obj)
    {
    }
    
    // To be created in order to listen 
    public BaseListenerData(SubjectType subjectType, int number)
    {
        this.subjectType = subjectType;
        this.number = number;
    }
    
    public virtual void Execute(BaseListenerData data)
    {
        var playerQuest = DataPlayer.GetModule<PlayerDailyQuest>();
        playerQuest.AddQuestProgress(data);
    }

    public SubjectType GetSubjectType()
    {
        return subjectType;
    }
}

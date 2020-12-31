using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchAdsProgress : QuestProgress
{
    public WatchAdsProgress(BaseQuestData data, long progress = 0, int state = 0) : base(data, progress, state)
    {
    }
    
    public WatchAdsProgress(QuestProgress data) : base(data)
    {
    }
    
    public override bool IsShouldNotify(BaseListenerData baseQuestData)
    {
        return baseQuestData.subjectType == SubjectType.WatchAds;
    }
}

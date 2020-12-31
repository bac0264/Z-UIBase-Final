using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchAdsListenerData : BaseListenerData
{
    public WatchAdsListenerData(object obj) : base(obj)
    {
        subjectType = SubjectType.WatchAds;
        PublisherService.ResgisterModule(subjectType, this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaListenerData : BaseListenerData
{
    public GachaListenerData(object obj) : base(obj)
    {
        subjectType = SubjectType.Gacha;
        PublisherService.ResgisterModule(subjectType, this);
    }
    //
    // public GachaListenerData(SubjectType subjectType, int number) : base(subjectType, number)
    // {
    //     this.subjectType = subjectType;
    //     this.number = number;
    // }
    
}

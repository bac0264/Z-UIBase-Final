using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginListenerData : BaseListenerData
{
    public LoginListenerData(object obj) : base(obj)
    {
        subjectType = SubjectType.Login;
        PublisherService.ResgisterModule(subjectType, this);
    }
    
    // public LoginListenerData(SubjectType subjectType, int number) : base(subjectType, number)
    // {
    //     this.subjectType = subjectType;
    //     this.number = number;
    // }
}

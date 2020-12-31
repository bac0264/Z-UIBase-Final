using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginProgress : QuestProgress
{
    public LoginProgress(BaseQuestData data, long progress = 0,int state = 0) : base (data, progress, state)
    {
    }

    public LoginProgress(QuestProgress data) : base (data)
    {
    }
    
    public override bool IsShouldNotify(BaseListenerData baseQuestData)
    {
        return baseQuestData.subjectType == SubjectType.Login;
    }
}

using Newtonsoft.Json;
using UnityEngine;


public enum QuestState
{
    Doing = 0,
    Done = 1,
    Claimed = 2,
}

[System.Serializable]
public class QuestProgress
{
    [JsonProperty("0")] public long progress;
    [JsonProperty("1")] public int state;
    [JsonProperty("2")] public int type;

    public QuestProgress()
    {
    }

    public QuestProgress(BaseQuestData data, long progress = 0, int state = 0)
    {
        this.state = state;
        type = data.type;
        this.progress = progress;
    }

    public QuestProgress(QuestProgress data)
    {
        state = data.state;
        type = data.type;
        progress = data.progress;
    }

    public QuestState GetState()
    {
        return (QuestState) state;
    }

    public virtual void ResetQuest()
    {
        progress = 0;
        state = (int) QuestState.Doing;
    }
    
    public virtual void Claim()
    {
        state = (int) QuestState.Claimed;
        DataPlayer.GetModule<PlayerDailyQuest>().Save();
    }

    public virtual void SetState(long required)
    {
        if (progress >= required && state == (int) QuestState.Doing) state = (int) QuestState.Done;
    }

    public virtual void AddProgress(BaseListenerData baseQuestData)
    {
        progress += baseQuestData.number;
    }

    public virtual bool IsShouldNotify(BaseListenerData baseQuestData)
    {
        return true;
    }

    public static QuestProgress CreateInstance(BaseQuestData data, long progress = 0, int state = 0)
    {
        switch ((QuestType) data.type)
        {
            case QuestType.LOGIN:
                return new LoginProgress(data, progress, state);
            case QuestType.GACHA:
                return new GachaProgress(data, progress, state);
            case QuestType.PLAY_CAMPAIGN:
                return new CampaignProgress(data, progress, state);
            case QuestType.WATCH_ADS:
                return new WatchAdsProgress(data, progress, state);
        }

        return null;
    }
}
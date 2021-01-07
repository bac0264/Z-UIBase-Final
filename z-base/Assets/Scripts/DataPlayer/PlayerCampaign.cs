using Newtonsoft.Json;
using UnityEngine;

public class PlayerCampaign
{
    [JsonProperty("player_campaign")]
    private PlayerCampaignSaveLoad dataCampaign;

    public PlayerCampaign()
    {
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString(KeyUtils.CAMPAIGN_DATA, JsonConvert.SerializeObject(dataCampaign));
    }

    public void Load()
    {
        dataCampaign = JsonConvert.DeserializeObject<PlayerCampaignSaveLoad>(PlayerPrefs.GetString(KeyUtils.CAMPAIGN_DATA));
        if (dataCampaign == null)
        {
            dataCampaign = new PlayerCampaignSaveLoad
            {
                currentStage = 101001,
                lastStagePass =  101001,
            };
        }
    }

    public int GetCurrentStage()
    {
        return dataCampaign.currentStage;
    }
    
    public int GetLastStagePass()
    {
        return dataCampaign.lastStagePass;
    }

    public void SetLastStagePass(int stage)
    {
        if (stage > dataCampaign.lastStagePass)
        {
            dataCampaign.lastStagePass = stage;
            Save();
        }
    }
    
    public void SetCurrentStage(int currentStage)
    {
        dataCampaign.currentStage = currentStage;
        Save();
    }
}

[System.Serializable]
public class PlayerCampaignSaveLoad{
    [JsonProperty("0")] public int currentStage;
    [JsonProperty("1")] public int lastStagePass;
}

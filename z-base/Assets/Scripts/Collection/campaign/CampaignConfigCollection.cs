using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StageState
{
    Completed = 0,
    Opening = 1,
    Lock = 2,
}

[System.Serializable]
public class CampaignStageData
{
    public int stage;
    public Reward[] rewards;

    public StageState GetState()
    {
        var lastId = DataPlayer.GetModule<PlayerCampaign>().GetLastStagePass();

        if (stage < lastId) return StageState.Completed;

        if (stage == lastId) return StageState.Opening;

        return StageState.Lock;
    }

    public static int GetStageIndex(int stage)
    {
        return stage % 100000 % 1000;
    }
}

[System.Serializable]
public class CampaignConfigCollection : ScriptableObject
{
    private CampaignStageData[] dataGroups;

    public CampaignWorldConfig worldConfig;

    public bool IsPassWorldWithId(int stage)
    {
        var modeId = CampaignModeConfig.GetModeId(stage);
        var mode = GetModeCampaignWithId(modeId);
        return mode.IsPassModeWithStage(stage);
    }

    public CampaignStageData GetNextStage(int lastStage)
    {
        var modeId = CampaignModeConfig.GetModeId(lastStage);

        // check mode with current stage
        var mode = GetModeCampaignWithId(modeId);
        if (mode == null) return null;

        // check map with current stage
        var mapId = CampaignMapConfig.GetMapId(lastStage);

        var map = mode.GetMapWithId(mapId);
        if (map == null) return null;

        var stage = map.GetNextStage(lastStage);
        if (stage != null)
        {
            return stage;
        }

        // check stage in next map
        mapId += 1;
        map = mode.GetMapWithId(mapId);
        if (map == null)
        {
            // if not exist next map, check stage in next mode
            modeId += 1;
            mode = GetModeCampaignWithId(modeId);
            if (mode == null) return null;

            map = mode.GetMapWithId(1);
            if (map == null) return null;

            stage = map.GetNextStage(lastStage);
            if (stage != null) return stage;
        }
        else
        {
            stage = map.GetNextStage(lastStage);
            if (stage != null)
            {
                return stage;
            }
        }

        return null;
    }

    public CampaignModeConfig GetModeCampaignWithId(int id)
    {
        return worldConfig.GetModeCampaignWithId(id);
    }

    public CampaignMapConfig GetMapCampaignConfigWithStageId(int stage)
    {
        return worldConfig.GetModeCampaignWithStageId(stage).GetMapWithStageId(stage);
    }

    public void Convert()
    {
        if (dataGroups.Length > 0)
        {
            var modeCount = CampaignModeConfig.GetModeId(dataGroups[dataGroups.Length - 1].stage);

            var modeConfigList = new CampaignWorldConfig();
            for (int i = 0; i < modeCount; i++)
            {
                var mode = new CampaignModeConfig();
                mode.modeId = i + 1;

                var mapElement = new CampaignMapConfig();
                mapElement.mapId = 1;
                mode.mapList.Add(mapElement);

                for (int j = 0; j < dataGroups.Length; j++)
                {
                    var data = dataGroups[j];
                    var modeId = CampaignModeConfig.GetModeId(data.stage);

                    if (modeId == mode.modeId)
                    {
                        var mapId = CampaignMapConfig.GetMapId(data.stage);
                        if (mapId == mapElement.mapId)
                        {
                            mapElement.stageList.Add(data);
                        }
                        else
                        {
                            mapElement = new CampaignMapConfig();
                            mapElement.mapId = mapId;
                            mapElement.stageList.Add(data);
                            mode.mapList.Add(mapElement);
                        }
                    }
                }

                modeConfigList.modeConfigList.Add(mode);
            }

            worldConfig = modeConfigList;
        }
    }
}


[System.Serializable]
public class CampaignWorldConfig
{
    public List<CampaignModeConfig> modeConfigList = new List<CampaignModeConfig>();

    public CampaignModeConfig GetModeCampaignWithId(int id)
    {
        for (int i = 0; i < modeConfigList.Count; i++)
        {
            if (id == modeConfigList[i].modeId) return modeConfigList[i];
        }

        return null;
    }

    public CampaignModeConfig GetModeCampaignWithStageId(int stageId)
    {
        var id = CampaignModeConfig.GetModeId(stageId);
        for (int i = 0; i < modeConfigList.Count; i++)
        {
            if (id == modeConfigList[i].modeId) return modeConfigList[i];
        }

        return null;
    }
}

public enum ModeState
{
    Completed = 0,
    Opening = 1,
    Lock = 2,
    CommingSoon = 3,
}

[System.Serializable]
public class CampaignModeConfig
{
    public int modeId;
    public List<CampaignMapConfig> mapList = new List<CampaignMapConfig>();

    public bool IsPassModeWithStage(int stage)
    {
        var count = mapList.Count;
        if (count > 0)
        {
            var map = this.mapList[count - 1];
            if (map.stageList.Count > 0)
            {
                var maxStage = map.stageList[map.stageList.Count - 1];
                return stage > maxStage.stage;
            }
        }

        return false;
    }

    public ModeState GetStateWithModeId()
    {
        var currentId = GetModeId(DataPlayer.GetModule<PlayerCampaign>().GetLastStagePass());
        if (currentId == 1000) return ModeState.CommingSoon;
        if (modeId < currentId) return ModeState.Completed;
        if (modeId == currentId) return ModeState.Opening;
        if (modeId > currentId) return ModeState.Lock;
        return ModeState.CommingSoon;
    }

    public CampaignMapConfig GetMapWithId(int mapId)
    {
        for (int i = 0; i < mapList.Count; i++)
        {
            if (mapId == mapList[i].mapId) return mapList[i];
        }

        return null;
    }

    public CampaignMapConfig GetMapWithStageId(int stageId)
    {
        var mapId = CampaignMapConfig.GetMapId(stageId);
        for (int i = 0; i < mapList.Count; i++)
        {
            if (mapId == mapList[i].mapId) return mapList[i];
        }

        return null;
    }

    public static int GetModeId(int stageId)
    {
        return stageId / 100000;
    }
}

public enum MapState
{
    Completed = 0,
    Opening = 1,
    Lock = 2,
}

[System.Serializable]
public class CampaignMapConfig
{
    public int mapId;
    public List<CampaignStageData> stageList = new List<CampaignStageData>();

    public CampaignStageData GetNextStage(int stage)
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            if (stage < stageList[i].stage) return stageList[i];
        }

        return null;
    }

    public CampaignStageData GetStageWithId(int stage)
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            if (stage == stageList[i].stage) return stageList[i];
        }

        return null;
    }

    public MapState GetState()
    {
        var lastMapId = GetMapId(DataPlayer.GetModule<PlayerCampaign>().GetLastStagePass());

        if (mapId < lastMapId)
            return MapState.Completed;

        if (mapId == lastMapId)
            return MapState.Opening;

        return MapState.Lock;
    }

    public static int GetMapId(int stageId)
    {
        return stageId % 100000 / 1000;
    }
}
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct MissionSaveData
{
    public int missionId;
    public int count;
    public bool success;
    public bool isComplete;
}

[Serializable]
public struct PreMissionData
{
    public int missionId;
    public bool Clear;
}

[Serializable]
public class SaveMissionData
{
    public float dailyPoint;
    public float weeklyPoint;
    public float monthlyPoint;
    public List<MissionSaveData> dailyMissions;
    public List<MissionSaveData> weeklyMissions;
    public List<MissionSaveData> monthlyMissions;
    public List<PreMissionData> preMissions;
}

public class MissionManager : Singleton<MissionManager>
{
    private Dictionary<int, MissionSaveData> missionProgress = new Dictionary<int, MissionSaveData>();
    private Dictionary<int, PreMissionData> preMissionProgress = new Dictionary<int, PreMissionData>();
    public float dailyPoints = 0f;
    public float weeklyPoints = 0f;
    public float monthlyPoints = 0f;
    private bool isAddQuitEvent = false;
    public bool IsSetUi { get; private set; } = false;

    public MissionTable missionTable;
    public List<MissionData> missionData;

    public int dailyMissionCount = 0;
    public int weeklyMissionCount = 0;
    public int monthlyMissionCount = 0;

    private async void Awake()
    {
        await GameManager.Instance.UniWaitTables();
        missionTable = DataTableMgr.GetMissionTable();
        missionData = new List<MissionData>(missionTable.GetAllMissions());
        await UniTask.WaitUntil(() => UtilityTime.isLoadComplete);

        if (!isAddQuitEvent)
        {
            Application.quitting -= SaveGameData;
            Application.quitting += SaveGameData;
            isAddQuitEvent = true;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            SaveGameData();
        }
    }

    public void AddMissionCountTargetId(int targetId, int count = 1)
    {
        foreach (var missionData in missionTable.GetAllMissions())
        {
            if (missionData.Target_ID == targetId)
            {
                AddMissionCount(missionData.Mission_ID, count);
                return;
            }
        }
    }

    public void AddMissionCount(int missionId, int count)
    {
        if (!missionProgress.ContainsKey(missionId))
        {
            missionProgress[missionId] = new MissionSaveData { missionId = missionId, count = 0, success = false, isComplete = false };
        }
        var missionSaveData = missionProgress[missionId];
        missionSaveData.count += count;

        var missionData = GetMissionData(missionId);

        if (missionSaveData.count >= missionData.Count)
        {
            missionSaveData.success = true;
            missionSaveData.count = missionData.Count;
        }

        missionProgress[missionId] = missionSaveData;
    }

    public int GetMissionCount(int missionId)
    {
        return missionProgress.ContainsKey(missionId) ? missionProgress[missionId].count : 0;
    }

    public List<MissionSaveData> GetMissionsByType(MissionDayTypes missionType)
    {
        List<MissionSaveData> missions = new List<MissionSaveData>();

        foreach (var missionData in missionProgress.Values)
        {
            if (GetMissionData(missionData.missionId).Mission_Type == (int)missionType)
            {
                missions.Add(missionData);
            }
        }
        return missions;
    }

    public void SaveGameData()
    {
        SaveMissionData gameData = new SaveMissionData
        {
            dailyPoint = dailyPoints,
            weeklyPoint = weeklyPoints,
            monthlyPoint = monthlyPoints,
            dailyMissions = new List<MissionSaveData>(),
            weeklyMissions = new List<MissionSaveData>(),
            monthlyMissions = new List<MissionSaveData>(),
            preMissions = new List<PreMissionData>(),
        };

        foreach (var progress in missionProgress.Values)
        {
            var missionData = GetMissionData(progress.missionId);
            if (missionData.Mission_Type == (int)MissionDayTypes.Daily)
                gameData.dailyMissions.Add(progress);
            else if (missionData.Mission_Type == (int)MissionDayTypes.Weekly)
                gameData.weeklyMissions.Add(progress);
            else if (missionData.Mission_Type == (int)MissionDayTypes.Monthly)
                gameData.monthlyMissions.Add(progress);
        }

        foreach (var preMission in preMissionProgress.Values)
        {
            gameData.preMissions.Add(preMission);
        }

        SaveLoadSystem.Save(gameData);
    }

    public void LoadGameData()
    {
        SaveMissionData gameData = SaveLoadSystem.MissionLoad();
        if (gameData == null)
        {
            return;
        }

        dailyPoints = gameData.dailyPoint;
        weeklyPoints = gameData.weeklyPoint;
        monthlyPoints = gameData.monthlyPoint;
        missionProgress.Clear();

        foreach (var saveData in gameData.dailyMissions)
        {
            missionProgress[saveData.missionId] = saveData;
            var missionData = GetMissionData(saveData.missionId);
        }
        dailyMissionCount = gameData.dailyMissions.Count;

        foreach (var saveData in gameData.weeklyMissions)
        {
            missionProgress[saveData.missionId] = saveData;
            var missionData = GetMissionData(saveData.missionId);
        }
        weeklyMissionCount = gameData.weeklyMissions.Count;

        foreach (var saveData in gameData.monthlyMissions)
        {
            missionProgress[saveData.missionId] = saveData;
            var missionData = GetMissionData(saveData.missionId);
        }
        monthlyMissionCount = gameData.monthlyMissions.Count;

        if (gameData.preMissions == null || gameData.preMissions.Count == 0)
        {
            foreach (var mission in missionData)
            {
                if (mission.Pre_Mission_ID != 0)
                {
                    preMissionProgress[mission.Pre_Mission_ID] = new PreMissionData
                    {
                        missionId = mission.Pre_Mission_ID,
                        Clear = false
                    };
                }
            }
        }
        else
        {
            foreach (var saveData in gameData.preMissions)
            {
                preMissionProgress[saveData.missionId] = saveData;
            }
        }
    }

    public MissionData GetMissionData(int missionId)
    {
        if (!missionTable.IsLoaded)
        {
            return MissionTable.defaultData;
        }

        var missionData = missionTable.Get(missionId);
        return missionData;
    }
    public void ResetMissions(MissionDayTypes missionType)
    {
        LoadGameData();
        List<MissionSaveData> missionsToReset = GetMissionsByType(missionType);

        for (int i = 0; i < missionsToReset.Count; i++)
        {
            var mission = missionsToReset[i];
            mission.count = 0;
            mission.success = false;
            mission.isComplete = false;
            missionProgress[mission.missionId] = mission;
        }
        SaveGameData();
    }
    public MissionSaveData GetMissionSaveData(int missionId)
    {
        if (missionProgress.ContainsKey(missionId))
        {
            return missionProgress[missionId];
        }
        return new MissionSaveData { missionId = missionId, count = 0, success = false, isComplete = false };
    }

    public void UpdateMissionSaveData(MissionSaveData updatedData)
    {
        if (missionProgress.ContainsKey(updatedData.missionId))
        {
            missionProgress[updatedData.missionId] = updatedData;   
        }
        else
        {
            missionProgress.Add(updatedData.missionId, updatedData);
        }
        if(preMissionProgress.ContainsKey(updatedData.missionId))
        {
            var preMissionData = preMissionProgress[updatedData.missionId];
            preMissionData.Clear = true;
            preMissionProgress[updatedData.missionId] = preMissionData;
        }
    }

    public bool CheckPreMissionId(int id)
    {
        if(preMissionProgress.ContainsKey(id))
        {
            if (preMissionProgress[id].Clear)
                return true;
        }
        return false;
    }
}


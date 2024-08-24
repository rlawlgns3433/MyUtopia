using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Serializable]
public struct MissionSaveData
{
    public int missionId;
    public int count;
    public bool success;
    public bool isComplete;
    public bool openMission;
    public bool available;
    public bool repeat;
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
    public List<bool> completeDailyMission;
}

public class MissionManager : Singleton<MissionManager>, ISingletonCreatable
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

    public List<bool> dailyMissionCheck = new List<bool>();

    private async void Start()
    {
        if (!ShouldBeCreatedInScene(SceneManager.GetActiveScene().name))
        {
            Destroy(gameObject);
            return;
        }
        await GameManager.Instance.UniWaitTables();
        missionTable = DataTableMgr.GetMissionTable();
        missionData = new List<MissionData>(missionTable.GetAllMissions());
        await UniTask.WaitUntil(() => UtilityTime.isLoadComplete);
        UtilityTime.Instance.SetMissionData();
        if (!isAddQuitEvent)
        {
            //Application.quitting -= SaveGameData;
            //Application.quitting += SaveGameData;
            isAddQuitEvent = true;
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();

        if (applicationIsQuitting)
            return;

        if (!ShouldBeCreatedInScene(SceneManager.GetActiveScene().name))
        {
            _instance = null;
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
                //return;
            }
        }
    }

    public void AddMissionCountMakeItem(int targetId, int count = 1)
    {
        foreach (var missionData in missionTable.GetAllMissions())
        {
            if(missionData.Check_type == (int)MissionCheckTypes.Make)
            {
                if (missionData.Target_ID == targetId)
                {
                    AddMissionCount(missionData.Mission_ID, count);
                    //return;
                }
                else if (missionData.Target_ID == 0)
                {
                    if(targetId == 0)
                    {
                        AddMissionCount(missionData.Mission_ID, count);
                    }
                }
            }

        }
    }

    public void AddMissionCountSellItem(int count = 1)
    {
        foreach (var missionData in missionTable.GetAllMissions())
        {
            if(missionData.Check_type == (int)MissionCheckTypes.Sell)
            {
                AddMissionCount(missionData.Mission_ID, count);
                //return;
            }
        }
    }
    public void AddMissionCountMurge(int count = 1)
    {
        foreach (var missionData in missionTable.GetAllMissions())
        {
            if (missionData.Check_type == (int)MissionCheckTypes.Murge)
            {
                AddMissionCount(missionData.Mission_ID, count);
                //return;
            }
        }
    }
    public void AddMissionCount(int missionId, int count)
    {
        if (FloorManager.Instance.touchManager.tutorial.gameObject.activeSelf)
            return;
<<<<<<< HEAD
        
=======

>>>>>>> CBTTest
        if (!missionProgress.ContainsKey(missionId))
        {
            missionProgress[missionId] = new MissionSaveData
            {
                missionId = missionId,
                count = 0,
                success = false,
                isComplete = false,
                openMission = false,
                available = false,
                repeat = true
            };
        }
        var missionSaveData = missionProgress[missionId];
        Debug.Log($"AddMissionCountmissionId{missionId}=>{missionSaveData.count} /////{missionProgress[missionId].count}");
        missionSaveData.count += count;

        var missionData = GetMissionData(missionId);

        if (missionSaveData.count >= missionData.Count)
        {
            missionSaveData.success = true;
            missionSaveData.count = missionData.Count;
        }
        if(missionData.Pre_Event_Type == 0 && missionData.Pre_Mission_ID == 0)
        {
            missionSaveData.openMission = true;
        }
        if(missionData.Available == true)
        {
            missionSaveData.available = true;
        }

        missionProgress[missionId] = missionSaveData;

        if (missionData.Pre_Mission_ID != 0)
        {
            preMissionProgress[missionData.Pre_Mission_ID] = new PreMissionData
            {
                missionId = missionData.Pre_Mission_ID,
                Clear = false
            };
        }
        
    }

    public int GetMissionCount(int missionId)
    {
        return missionProgress.ContainsKey(missionId) ? missionProgress[missionId].count : 0;
    }

    public List<MissionSaveData> GetMissionsByType(MissionDayTypes missionType)
    {
        List<MissionSaveData> missions = new List<MissionSaveData>();
        Debug.Log($"MissionManager missionProgressCount= {missionProgress.Count}");
        foreach (var missionData in missionProgress.Values)
        {
            if (GetMissionData(missionData.missionId).Mission_Type == (int)missionType)
            {
                missions.Add(missionData);
                Debug.Log($"GetMissionSaveData {missionData.count}");
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
            completeDailyMission = new List<bool>()
        };
        gameData.completeDailyMission = dailyMissionCheck;
        foreach (var progress in missionProgress.Values)
        {
            Debug.Log($"Mission : {progress.missionId}, Count : {progress.count}");

            var missionData = GetMissionData(progress.missionId);
            if (missionData.Mission_Type == (int)MissionDayTypes.Daily)
            {
                Debug.Log($"Mission : {progress.missionId}, Count : {progress.count}");
                gameData.dailyMissions.Add(progress);
            }
            else if (missionData.Mission_Type == (int)MissionDayTypes.Weekly)
                gameData.weeklyMissions.Add(progress);
            else if (missionData.Mission_Type == (int)MissionDayTypes.Monthly)
                gameData.monthlyMissions.Add(progress);
            else
            {
                Debug.Log("ERR Mission");
            }
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
            gameData = SaveLoadSystem.EmptyMissionLoad(SaveLoadSystem.SaveType.EmptyMission);
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
        if(missionProgress.Count <=0)
        {
            foreach (var mission in missionData)
            {
                missionProgress[mission.Mission_ID] = new MissionSaveData
                {
                    missionId = mission.Mission_ID,
                    count = 0,
                    success = false,
                    isComplete = false,
                    openMission = false,
                    available = false,
                    repeat = true

                };
                var missionData = GetMissionData(mission.Mission_ID);
                var tempMission = missionProgress[mission.Mission_ID];
                if (missionData.Available == true)
                {
                    tempMission.available = true;
                }
                if (missionData.Pre_Event_Type == 0 && missionData.Pre_Mission_ID == 0)
                {
                    tempMission.openMission = true;
                }
                else if (missionData.Pre_Mission_ID != 0)
                {
                    if (CheckPreMissionId(missionData.Pre_Mission_ID))
                    {
                        tempMission.openMission = true;
                    }
                }
                else if (missionData.Pre_Event_Type == 2)
                {
                    var targetFloor = FloorManager.Instance.GetFloor($"B{missionData.Pre_Event / 100 % 10}");
                    if (targetFloor.FloorStat.Floor_ID >= missionData.Pre_Event)
                    {
                        tempMission.openMission = true;
                    }
                }
                else if (missionData.Pre_Event_Type == 3)
                {
                    var targetFloor = FloorManager.Instance.GetFloor("B3");
                    if (targetFloor.buildings != null)
                    {
                        foreach (var building in targetFloor.buildings)
                        {
                            var craftBuilding = building as CraftingBuilding;
                            if (craftBuilding == null)
                                continue;
                            var value = craftBuilding.BuildingStat.Building_ID / 100 % 100;
                            if (value >= missionData.Pre_Event / 100 % 100)
                            {
                                tempMission.openMission = true;
                            }
                        }
                    }
                }
                missionProgress[mission.Mission_ID] = tempMission;
            }
        }
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
        if(gameData.completeDailyMission.Count != 0)
        {
            dailyMissionCheck = gameData.completeDailyMission;
        }
        else
        {
            dailyMissionCheck = new List<bool>(3);
            for (int i = 0; i < 3; i++)
            {
                dailyMissionCheck.Add(false);
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
        switch (missionType)
        {
            case MissionDayTypes.Daily:
                dailyPoints = 0;
                for(int i = 0; i < dailyMissionCheck.Count; ++i)
                {
                    dailyMissionCheck[i] = false;
                }
                break;
            case MissionDayTypes.Weekly:
                weeklyPoints = 0;
                break;
            case MissionDayTypes.Monthly:
                monthlyPoints = 0;
                break;
        }
        foreach (var mission in missionData)
        {
            if(preMissionProgress.ContainsKey(mission.Mission_ID))
            {
                if (preMissionProgress[mission.Mission_ID].Clear)
                {
                    foreach(var value in missionData)
                    {
                        if(value.Pre_Mission_ID == mission.Mission_ID)
                            GetMissionSaveData(mission.Mission_ID);
                    }
                }
            }
        }
        List<MissionSaveData> missionsToReset = GetMissionsByType(missionType);
        for (int i = 0; i < missionsToReset.Count; i++)
        {
            var mission = missionsToReset[i];
            var missionData = GetMissionData(mission.missionId);
            mission.count = 0;
            mission.success = false;
            mission.isComplete = false;
            mission.openMission = false;
            mission.available = false;
            if(missionData.Available == true)
            {
                mission.available = true;
            }
            if(missionData.Pre_Event_Type == 0 && missionData.Pre_Mission_ID == 0)
            {
                mission.openMission = true;
            }
            else if(missionData.Pre_Mission_ID != 0)
            {
                if(CheckPreMissionId(missionData.Pre_Mission_ID))
                {
                    mission.openMission = true;
                }
            }
            else if(missionData.Pre_Event_Type == 2)
            {
                var targetFloor = FloorManager.Instance.GetFloor($"B{missionData.Pre_Event / 100 % 10}");
                if(targetFloor.FloorStat.Floor_ID >= missionData.Pre_Event)
                {
                    mission.openMission = true;
                }
            }
            else if(missionData.Pre_Event_Type == 3)
            {
                var targetFloor = FloorManager.Instance.GetFloor("B3");
                if(targetFloor.buildings != null)
                {
                    foreach (var building in  targetFloor.buildings)
                    {
                        var craftBuilding = building as CraftingBuilding;
                        if (craftBuilding == null)
                            continue;
                        var value = craftBuilding.BuildingStat.Building_ID / 100 % 100;
                        if(value >= missionData.Pre_Event / 100 % 100)
                        {
                            mission.openMission = true;
                        }
                    }
                }
            }
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
        return new MissionSaveData { missionId = missionId, count = 0, success = false, isComplete = false, openMission = false, available = false, repeat = true };
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

    public bool ShouldBeCreatedInScene(string sceneName)
    {
        return sceneName == "SampleScene";
    }
}


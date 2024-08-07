using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[Serializable]
public struct SaveMissionData
{
    public float dailyPoint;
    public float weeklyPoint;
    public float monthlyPoint;
    public List<MissionSaveData> dailyMissions;
    public List<MissionSaveData> weeklyMissions;
    public List<MissionSaveData> monthlyMissions;
}
public class UiMissionList : MonoBehaviour
{
    private MissionTable missionTable;
    public UiMission uiMissionPrefab;
    public Transform missionParent;
    private List<UiMission> missionList;
    private List<MissionData> missionData;
    public Slider missionSlider;
    public float sliderMaxValue;
    public GameObject checkPointHalf;
    public GameObject checkPointTwoThird;
    public GameObject checkPointMax;
    private float halfPoint;
    private float twoThirdPoint;
    private float maxPoint;
    private bool halfPointCheck = false;
    private bool twoThirdCheck = false;
    private bool maxPointCheck = false;
    private float dailyPoint = 0f;
    private float weeklyPoint = 0f;
    private float monthlyPoint = 0f;
    private MissionTypes missionType;

    private void Awake()
    {
        missionTable = DataTableMgr.GetMissionTable();
        missionList = new List<UiMission>();
        missionData = new List<MissionData>();
        missionData = missionTable.GetAllMissions();
        missionType = MissionTypes.Daily;
    }

    private void OnEnable()
    {
        if (missionList.Count == 0)
        {
            SetDailyMissionData();
        }
        else
        {
            foreach (var mission in missionList)
            {
                mission.UpDateMission();
            }
            CheckMissionPoint();
        }
    }

    private void OnDisable()
    {
        SaveGameData();
        //юс╫ц
    }
    //private void OnApplicationPause(bool pauseStatus)
    //{
    //    if (pauseStatus)
    //    {
    //        Debug.Log("MissionSave....");

    //    }
    //}

    private void SetDailyMissionData()
    {
        ClearMissionList();
        foreach (var mission in missionData)
        {
            if (mission.Mission_Type == (int)MissionTypes.Daily)
            {
                bool create = false;
                var floor = FloorManager.Instance.GetFloor($"B{mission.Target_ID / 10000 % 100}");
                foreach (var building in floor.buildings)
                {
                    if (mission.Target_ID == building.buildingId && !building.BuildingStat.IsLock)
                    {
                        create = true;
                        break;
                    }
                }
                if (create)
                {
                    var missionInstance = Instantiate(uiMissionPrefab, missionParent);
                    missionInstance.SetData(mission);
                    missionList.Add(missionInstance);
                }
            }
        }
        SetSliderCheckPoint();
    }

    private void SetWeeklyMissionData()
    {
        foreach (var mission in missionData)
        {
            if (mission.Mission_Type == (int)MissionTypes.Weekly)
            {
                bool create = false;
                var floor = FloorManager.Instance.GetFloor($"B{mission.Target_ID / 10000 % 100}");
                foreach (var building in floor.buildings)
                {
                    if (mission.Target_ID == building.buildingId && !building.BuildingStat.IsLock)
                    {
                        create = true;
                        break;
                    }
                }
                if (create)
                {
                    var missionInstance = Instantiate(uiMissionPrefab, missionParent);
                    missionInstance.SetData(mission);
                    missionList.Add(missionInstance);
                }
            }
        }
    }

    private void SetMonthlyMissionData()
    {
        foreach (var mission in missionData)
        {
            if (mission.Mission_Type == (int)MissionTypes.Monthly)
            {
                bool create = false;
                var floor = FloorManager.Instance.GetFloor($"B{mission.Target_ID / 10000 % 100}");
                foreach (var building in floor.buildings)
                {
                    if (mission.Target_ID == building.buildingId && !building.BuildingStat.IsLock)
                    {
                        create = true;
                        break;
                    }
                }
                if (create)
                {
                    var missionInstance = Instantiate(uiMissionPrefab, missionParent);
                    missionInstance.SetData(mission);
                    missionList.Add(missionInstance);
                }
            }
        }
    }

    private void ClearMissionList()
    {
        foreach (var mission in missionList)
        {
            Destroy(mission.gameObject);
        }
        missionList.Clear();
    }

    public void UpdateSliderValue(float value)
    {
        if (missionType == MissionTypes.Daily)
        {
            dailyPoint += value;
            missionSlider.value = dailyPoint;
            CheckMissionPoint();
        }
        else if (missionType == MissionTypes.Weekly)
        {
            weeklyPoint += value;
            missionSlider.value = weeklyPoint;
            CheckMissionPoint();
        }
        else if (missionType == MissionTypes.Monthly)
        {
            monthlyPoint += value;
            missionSlider.value = monthlyPoint;
            CheckMissionPoint();
        }
    }

    private void SetSliderCheckPoint()
    {
        halfPoint = missionSlider.maxValue / 2;
        twoThirdPoint = missionSlider.maxValue * 2 / 3;
        maxPoint = missionSlider.maxValue;

        checkPointHalf.GetComponentInChildren<TextMeshProUGUI>().text = ((int)halfPoint).ToString();
        checkPointTwoThird.GetComponentInChildren<TextMeshProUGUI>().text = ((int)twoThirdPoint).ToString();
        checkPointMax.GetComponentInChildren<TextMeshProUGUI>().text = ((int)maxPoint).ToString();
    }

    private void CheckMissionPoint()
    {
        if (halfPointCheck && twoThirdCheck && maxPointCheck)
            return;

        if (missionSlider.value >= halfPoint && !halfPointCheck)
        {
            SetCheckpoint(checkPointHalf, ref halfPointCheck);
        }
        if (missionSlider.value >= twoThirdPoint && !twoThirdCheck)
        {
            SetCheckpoint(checkPointTwoThird, ref twoThirdCheck);
        }
        if (missionSlider.value >= maxPoint && !maxPointCheck)
        {
            SetCheckpoint(checkPointMax, ref maxPointCheck);
        }
    }

    private void SetCheckpoint(GameObject checkpoint, ref bool checkFlag)
    {
        var image = checkpoint.GetComponent<Image>();
        image.color = Color.green;
        var button = checkpoint.GetComponentInChildren<Button>();
        button.interactable = true;
        button.onClick.AddListener(AddCurrencyValue);
        checkFlag = true;
    }

    public void AddCurrencyValue()
    {
        Debug.Log($"Slider value: {missionSlider.value}");
    }

    public void SetMissionType(MissionTypes missionTypes)
    {
        this.missionType = missionTypes;
    }

    private void SaveGameData()
    {
        List<MissionSaveData> dailyMissions = new List<MissionSaveData>();
        List<MissionSaveData> weeklyMissions = new List<MissionSaveData>();
        List<MissionSaveData> monthlyMissions = new List<MissionSaveData>();

        foreach (var mission in missionList)
        {
            switch (mission.missionData.Mission_Type)
            {
                case (int)MissionTypes.Daily:
                    dailyMissions.Add(mission.GetSaveData());
                    break;
                case (int)MissionTypes.Weekly:
                    weeklyMissions.Add(mission.GetSaveData());
                    break;
                case (int)MissionTypes.Monthly:
                    monthlyMissions.Add(mission.GetSaveData());
                    break;
            }
        }

        SaveMissionData gameData = new SaveMissionData
        {
            dailyPoint = dailyPoint,
            weeklyPoint = weeklyPoint,
            monthlyPoint = monthlyPoint,
            dailyMissions = dailyMissions,
            weeklyMissions = weeklyMissions,
            monthlyMissions = monthlyMissions
        };
        SaveLoadSystem.Save(gameData);
    }

    private void LoadGameData()
    {
        SaveMissionData gameData = SaveLoadSystem.MissionLoad();
        if (gameData.dailyMissions == null)
        {
            return;
        }

        dailyPoint = gameData.dailyPoint;
        weeklyPoint = gameData.weeklyPoint;
        monthlyPoint = gameData.monthlyPoint;

        foreach (var saveData in gameData.dailyMissions)
        {
            var missionDataInstance = missionTable.Get(saveData.missionId);
            if (!missionDataInstance.Equals(MissionTable.defaultData))
            {
                var missionInstance = Instantiate(uiMissionPrefab, missionParent);
                missionInstance.SetData(missionDataInstance);
                missionInstance.SetSaveData(saveData);
                missionList.Add(missionInstance);
            }
        }

        foreach (var saveData in gameData.weeklyMissions)
        {
            var missionDataInstance = missionTable.Get(saveData.missionId);
            if (!missionDataInstance.Equals(MissionTable.defaultData))
            {
                var missionInstance = Instantiate(uiMissionPrefab, missionParent);
                missionInstance.SetData(missionDataInstance);
                missionInstance.SetSaveData(saveData);
                missionList.Add(missionInstance);
            }
        }

        foreach (var saveData in gameData.monthlyMissions)
        {
            var missionDataInstance = missionTable.Get(saveData.missionId);
            if (!missionDataInstance.Equals(MissionTable.defaultData))
            {
                var missionInstance = Instantiate(uiMissionPrefab, missionParent);
                missionInstance.SetData(missionDataInstance);
                missionInstance.SetSaveData(saveData);
                missionList.Add(missionInstance);
            }
        }
    }
}

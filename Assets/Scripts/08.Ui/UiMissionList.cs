using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiMissionList : MonoBehaviour
{
    public UiMission uiMissionPrefab;
    public Transform missionParent;
    public MissionDayTypes missionType;
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
    private List<UiMission> dailyMissionList = new List<UiMission>(); // 리스트 초기화

    private bool missionsGenerated = false;

    private void OnEnable()
    {
        missionType = MissionDayTypes.Daily;

        if (!missionsGenerated) // 미션이 아직 생성되지 않았을 때만 생성
        {
            LoadAndDisplayMissions();
            missionsGenerated = true;
        }

        SetSliderCheckPoint();
        UpdateSlider();

        if (dailyMissionList.Count > 0)
        {
            foreach (var mission in dailyMissionList)
            {
                mission.UpDateMission();
                mission.SetButton();
            }
        }
    }

    public void UpdateSliderValue(float value)
    {
        Debug.Log($"Adding {value} points to {missionType} missions.");

        if (missionType == MissionDayTypes.Daily)
        {
            MissionManager.Instance.dailyPoints += value;
            missionSlider.value = MissionManager.Instance.dailyPoints;
        }
        else if (missionType == MissionDayTypes.Weekly)
        {
            MissionManager.Instance.weeklyPoints += value;
            missionSlider.value = MissionManager.Instance.weeklyPoints;
        }
        else if (missionType == MissionDayTypes.Monthly)
        {
            MissionManager.Instance.monthlyPoints += value;
            missionSlider.value = MissionManager.Instance.monthlyPoints;
        }

        CheckMissionPoint();
    }


    public void UpdateSlider()
    {
        if (missionType == MissionDayTypes.Daily)
        {
            missionSlider.value = MissionManager.Instance.dailyPoints;
        }
        else if (missionType == MissionDayTypes.Weekly)
        {
            missionSlider.value = MissionManager.Instance.weeklyPoints;
        }
        else if (missionType == MissionDayTypes.Monthly)
        {
            missionSlider.value = MissionManager.Instance.monthlyPoints;
        }

        CheckMissionPoint();
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
        // 여기에 포인트에 따른 보상 또는 추가 기능을 구현합니다.
    }

    private void LoadAndDisplayMissions()
    {
        ClearMissionList();
        List<MissionSaveData> missions = MissionManager.Instance.GetMissionsByType(missionType);

        if (missions.Count > 0)
        {
            Debug.Log($"Loaded {missions.Count} missions of type {missionType}");
        }
        else
        {
            CreateMissions();
            missions = MissionManager.Instance.GetMissionsByType(missionType);
        }

        foreach (var missionSaveData in missions)
        {
            var missionData = MissionManager.Instance.GetMissionData(missionSaveData.missionId);

            if (missionData.Mission_ID == default)
            {
                continue;
            }

            var missionInstance = Instantiate(uiMissionPrefab, missionParent);
            missionInstance.SetData(missionData);
            missionInstance.SetSaveData(missionSaveData);
            dailyMissionList.Add(missionInstance);
        }
    }

    private void CreateMissions()
    {
        foreach (var mission in MissionManager.Instance.missionData)
        {
            if (mission.Mission_Type == (int)missionType)
            {
                if(mission.Pre_Mission_ID != 0)
                {
                    if(!MissionManager.Instance.CheckPreMissionId(mission.Pre_Mission_ID))
                    {
                        continue;
                    }
                }

                //if(mission.Pre_Event_Type == 2)
                //{
                //    var targetId = mission.Pre_Event;
                //}
                MissionManager.Instance.AddMissionCount(mission.Mission_ID, 0);
            }
        }
        MissionManager.Instance.SaveGameData();
    }

    private void ClearMissionList()
    {
        foreach (Transform child in missionParent)
        {
            if (child.CompareTag("Mission"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}

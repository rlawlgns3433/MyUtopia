using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private void Awake()
    {
        missionTable = DataTableMgr.GetMissionTable();
        missionList = new List<UiMission>();
        missionData = new List<MissionData>();
        missionData = missionTable.GetAllMissions();
        for (int i = 0; i < missionData.Count; i++)
        {
            Debug.Log($"missionDataLoad{i} = {missionData[i].Mission_ID}");
        }
        SetDailyMissionData();
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

    private void SetDailyMissionData()
    {
        int missionCount = 0;
        foreach(var mission in missionData)
        {
            if(mission.Mission_Type == (int)MissionTypes.Daily)
            {
                missionCount++;
            }
        }
        for(int i = 0; i<missionCount; i++)
        {
            bool create = false;
            var floor = FloorManager.Instance.GetFloor($"B{missionData[i].Target_ID / 10000 % 100}");
            foreach(var building in floor.buildings)
            {
                if (missionData[i].Target_ID == building.buildingId)
                {
                    if(!building.BuildingStat.IsLock)
                    {
                        create = true;
                    }
                }
            }
            if(create)
            {
                missionList.Add(Instantiate(uiMissionPrefab, missionParent));
                missionList[i].SetData(missionData[i]);
            }
        }
        SetSliderCheckPoint();
    }

    private void ClearMissionList()
    {
        foreach(var mission in missionList)
        {
            Destroy(mission.gameObject);
        }
    }

    public void UpdateSliderValue(float value)
    {
        missionSlider.value += value;
        CheckMissionPoint();
    }

    private void SetSliderCheckPoint()
    {
        var halfText = checkPointHalf.GetComponentInChildren<TextMeshProUGUI>();
        halfPoint = missionSlider.maxValue / 2;
        halfText.text = ((int)halfPoint).ToString();
        var twoThirdText = checkPointTwoThird.GetComponentInChildren<TextMeshProUGUI>();
        twoThirdPoint = missionSlider.maxValue - (missionSlider.maxValue / 4);
        twoThirdText.text = ((int)twoThirdPoint).ToString();
        var maxText = checkPointMax.GetComponentInChildren<TextMeshProUGUI>();
        maxPoint = missionSlider.maxValue;
        maxText.text = maxPoint.ToString();
    }

    private void CheckMissionPoint()
    {
        if (halfPointCheck && twoThirdCheck && maxPointCheck)
            return;
        if (missionSlider != null)
        {
            if(!halfPointCheck&&missionSlider.value >= halfPoint)
            {
                var image = checkPointHalf.GetComponent<Image>();
                image.color = Color.green;
                var button = checkPointHalf.GetComponentInChildren<Button>();
                button.interactable = true;
                button.onClick.AddListener(AddCurrencyValue);
                halfPointCheck = true;
            }
            if(!twoThirdCheck&&missionSlider.value >= twoThirdPoint)
            {
                var image = checkPointTwoThird.GetComponent<Image>();
                image.color = Color.green;
                var button = checkPointTwoThird.GetComponentInChildren<Button>();
                button.interactable = true;
                button.onClick.AddListener(AddCurrencyValue);
                twoThirdCheck = true;
            }
            if(!maxPointCheck&&missionSlider.value >= missionSlider.maxValue)
            {
                var image = checkPointMax.GetComponent<Image>();
                image.color = Color.green;
                var button = checkPointMax.GetComponentInChildren<Button>();
                button.interactable = true;
                button.onClick.AddListener(AddCurrencyValue);
                maxPointCheck = true;
            }
        }
    }
    
    public void AddCurrencyValue()
    {
        Debug.Log($"Slider!!{missionSlider.value}");
    }
}

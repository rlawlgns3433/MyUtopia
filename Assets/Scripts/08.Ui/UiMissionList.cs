using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    //private bool halfPointCheck = false;
    //private bool twoThirdCheck = false;
    //private bool maxPointCheck = false;
    private List<UiMission> dailyMissionList = new List<UiMission>(); // ����Ʈ �ʱ�ȭ
    public ParticleSystem ps;
    private bool missionsGenerated = false;
    private List<bool> checkPoints = new List<bool>();
    private bool isPlayingPs = false;
    private void OnEnable()
    {
        missionType = MissionDayTypes.Daily;
        if (!missionsGenerated)
        {
            LoadAndDisplayMissions();
            checkPoints = MissionManager.Instance.dailyMissionCheck;
            foreach (var m in checkPoints)
            {
                Debug.Log($"saveCheckPoints{m}");
            }
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

    private void Start()
    {

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
        twoThirdPoint = missionSlider.maxValue * 0.75f;
        maxPoint = missionSlider.maxValue;

        //checkPointHalf.GetComponentInChildren<TextMeshProUGUI>().text = ((int)halfPoint).ToString();
        //checkPointTwoThird.GetComponentInChildren<TextMeshProUGUI>().text = ((int)twoThirdPoint).ToString();
        //checkPointMax.GetComponentInChildren<TextMeshProUGUI>().text = ((int)maxPoint).ToString();
        var reward = DataTableMgr.GetRewardTable().Get(12101001);
        checkPointHalf.GetComponentInChildren<TextMeshProUGUI>().text = reward.Reward1_Value;
        checkPointTwoThird.GetComponentInChildren<TextMeshProUGUI>().text = reward.Reward1_Value;
        checkPointMax.GetComponentInChildren<TextMeshProUGUI>().text = reward.Reward1_Value;
    }

    private void CheckMissionPoint()
    {
        checkPoints = MissionManager.Instance.dailyMissionCheck;
        if(checkPoints.Count <= 0)
        {
            for(int i = 0; i < 3; ++i)
            {
                checkPoints.Add(false);
                Debug.Log($"checkPoints List Count {checkPoints.Count}");
            }
        }
        if (checkPoints[0] && checkPoints[1] && checkPoints[2])
            return;

        if (missionSlider.value >= halfPoint && !checkPoints[0])//���� �˻� ��� ����? �����Ҷ� �� �Ҵ��ϴµ�...
        {
            SetCheckpoint(checkPointHalf, 0);
        }
        if (missionSlider.value >= twoThirdPoint && !checkPoints[1])
        {
            SetCheckpoint(checkPointTwoThird, 1);
        }
        if (missionSlider.value >= maxPoint && !checkPoints[2])
        {
            SetCheckpoint(checkPointMax, 2);
        }
    }

    private void SetCheckpoint(GameObject checkpoint, int index)
    {
        var button = checkpoint.GetComponent<Button>();
        button.interactable = true;
        button.onClick.AddListener(() => AddCurrencyValue(index));
    }

    public void AddCurrencyValue(int index)
    {
        if (isPlayingPs)
            return;
        var checkpoint = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        var pos = checkpoint.transform.position;
        ParticleSystemEmit(ps, pos, index).Forget();
        var image = checkpoint.GetComponent<Image>();
        image.color = Color.black;
        Debug.Log($"Slider value: {missionSlider.value}");
        checkpoint.interactable = false;
        checkpoint.onClick.RemoveListener(() => AddCurrencyValue(index));

    }

    public async UniTask ParticleSystemEmit(ParticleSystem ps, Vector2 pos, int index)
    {
        if (ps != null)
        {
            isPlayingPs = true;
            ps.transform.position = pos;
            ps.Emit(5);
            await UniTask.WaitUntil(() => !ps.IsAlive(true));
            isPlayingPs = false;
            CurrencyManager.currency[CurrencyType.Diamond] += new BigNumber(5);
            MissionManager.Instance.dailyMissionCheck[index] = true;
        }
    }

    private void LoadAndDisplayMissions()
    {
        ClearMissionList();
        List<MissionSaveData> missions = MissionManager.Instance.GetMissionsByType(missionType);
        Debug.Log($"missionCount // {missions.Count}");
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

            if (!missionSaveData.openMission)
                continue;
            if (!missionSaveData.available)
                continue;
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
                MissionManager.Instance.AddMissionCount(mission.Mission_ID, 0);
            }
        }
        MissionManager.Instance.ResetMissions(MissionDayTypes.Daily);
        //MissionManager.Instance.ResetMissions(MissionDayTypes.Weekly); // �ְ� ���� �߰� �� �ּ� ����
        //MissionManager.Instance.ResetMissions(MissionDayTypes.Monthly);
        //MissionManager.Instance.SaveGameData();
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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiMission : MonoBehaviour
{
    public MissionData missionData;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI missionDescText;
    public TextMeshProUGUI countText;
    public Button button;
    private int count;
    private bool isComplete = false;

    public void SetData(MissionData data)
    {
        missionData = data;
        count = MissionManager.Instance.GetMissionCount(missionData.Mission_ID);
        UpdateUI();
    }

    public void SetSaveData(MissionSaveData saveData)
    {
        count = saveData.count;
        isComplete = saveData.isComplete;
        UpdateUI();
    }

    private void UpdateUI()
    {
        difficultyText.text = missionData.GetDifficulty();
        missionDescText.text = missionData.GetDesc();
        countText.text = $"{count}/{missionData.Count}";
        SetButton();
    }

    public void UpDateMission()
    {
        count = MissionManager.Instance.GetMissionCount(missionData.Mission_ID);
        UpdateUI();
    }

    public void SetButton()
    {
        var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        // 리스너를 추가하기 전에 기존 리스너 제거
        button.onClick.RemoveAllListeners();

        if (count < missionData.Count)
        {
            button.onClick.AddListener(Move);
            buttonText.text = StringTextFormatKr.Move;
        }
        else if (!isComplete && count >= missionData.Count)
        {
            button.onClick.AddListener(MissionClear);
            button.GetComponent<Image>().color = Color.green;
            buttonText.text = StringTextFormatKr.Success;
        }
        else if (isComplete)
        {
            button.interactable = false;
            button.GetComponent<Image>().color = Color.white;
            buttonText.text = StringTextFormatKr.Completed;
        }
    }

    private void Move()
    {
        var floor = missionData.Target_ID / 10000 % 100;
        FloorManager.Instance.MoveToSelectFloor($"B{floor}");
        UiManager.Instance.ShowMainUi();
    }

    private void MissionClear()
    {
        isComplete = true;
        Debug.Log($"Mission {missionData.Mission_ID} cleared! Adding {missionData.Today_Mission_Point} points.");
        UiManager.Instance.uiMission.UpdateSliderValue(missionData.Today_Mission_Point);

        var missionSaveData = MissionManager.Instance.GetMissionSaveData(missionData.Mission_ID);
        missionSaveData.isComplete = true;
        MissionManager.Instance.UpdateMissionSaveData(missionSaveData);

        UpdateUI();
        MissionManager.Instance.SaveGameData();
    }
}

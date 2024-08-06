using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiMission : MonoBehaviour
{
    public MissionData missionData;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI missionDescText;
    public TextMeshProUGUI countText;
    public int count;
    public Button button;
    private bool success = false;
    private bool isComplete = false;

    public void SetData(MissionData data)
    {
        missionData = data;
        difficultyText.text = missionData.Difficulty.ToString();
        missionDescText.text = missionData.GetDesc();
        countText.text = $"0/{missionData.Count}";
        SetButton();
    }

    public void SetButton()
    {
        var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if(!success && count < missionData.Count)
        {
            button.onClick.AddListener(Move);
            button.onClick.AddListener(UiManager.Instance.ShowMainUi);
            buttonText.text = ButtonText.Move;
        }
        else if(success && count >= missionData.Count&&!isComplete)
        {
            button.GetComponent<Image>().color = Color.green;
            button.onClick.RemoveListener(Move);
            button.onClick.RemoveListener(UiManager.Instance.ShowMainUi);
            button.onClick.AddListener(MissionClear);
            buttonText.text = ButtonText.Success;
        }
        else if(isComplete)
        {
            button.GetComponent<Image>().color = Color.white;
            button.onClick.RemoveListener(MissionClear);
            button.interactable = false;
            buttonText.text = ButtonText.Completed;
        }
    }

    private void Move()
    {
        var floor = missionData.Target_ID / 10000 % 100;
        FloorManager.Instance.MoveToSelectFloor($"B{floor}");
    }

    private void MissionClear()
    {
        UiManager.Instance.uiMission.UpdateSliderValue(missionData.Today_Mission_Point);
        isComplete = true;
        SetButton();
    }

    public void UpDateMission()
    {
        count = MissionManager.Instance.GetMissionCount(missionData.Target_ID);
        if(count >= missionData.Count)
        {
            count = missionData.Count;
            success = true;
        }
        countText.text = $"{count}/{missionData.Count}";
        SetButton();
    }
}

using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class ClockFormatTimer : MonoBehaviour
{
    //private static readonly string formatTimer = "전체 갱신까지 {0:00}:{1:00}:{2:00}";
    private static readonly string formatTimer = "{0:00}:{1:00}:{2:00}";
    public int timerDuration = 3600; 

    public TextMeshProUGUI timerText;
    public bool canStartTimer = true;
    public TimeSpan timespan;
    public Tween timerTween;
    public float remainingTime;
    private float startTime;
    private IGrowable growable;
    private IUISetupable uISetupable;

    private void Start()
    {
        growable = GetComponent<IGrowable>();
        uISetupable = GetComponent<IUISetupable>();
    }

    public void StartClockTimer()
    {
        if (!canStartTimer)
            return;

        if(growable == null)
            growable = GetComponent<IGrowable>();

        if(uISetupable == null)
            uISetupable = GetComponent<IUISetupable>();

        timerText.gameObject.SetActive(true);
        StartClockTimer(timerDuration);
    }

    private void StartClockTimer(float duration)
    {
        remainingTime = duration;
        startTime = Time.time;

        timerTween = DOTween.To(() => remainingTime, x => remainingTime = x, 0, duration)
            .SetEase(Ease.Linear)
            .OnUpdate(UpdateTimerUI)
            .OnComplete(OnTimerComplete);
    }

    private void UpdateTimerUI()
    {
        float elapsedTime = Time.time - startTime;
        remainingTime = timerDuration - elapsedTime;

        int hours = Mathf.FloorToInt(remainingTime / 3600);
        int minutes = Mathf.FloorToInt((remainingTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        growable.UpgradeTimeLeft = Mathf.FloorToInt(remainingTime);

        timerText.text = string.Format(formatTimer, hours, minutes, seconds);
    }

    private void OnTimerComplete()
    {
        // 타이머 사용하는 곳
        // 단골 게시판 SetPatronUi, 건물 업그레이드 SetBuildingUi, 시설물 업그레이드 SetFurnitureUi, 계층 업그레이드 SetFloorUi = > SetUi
        timerText.text = string.Format(formatTimer, 0, 0, 0);

        if(growable.IsUpgrading)
        {
            growable.LevelUp();
            uISetupable.FinishUpgrade();
            growable.IsUpgrading = false;
            uISetupable.SetFinishUi();
        }
        // 타이머 끝났을 때 작업
    }

    public void RestartTimer()
    {
        if (timerTween != null && timerTween.IsPlaying())
        {
            timerTween.Kill();
        }
        SetDayTimer();
        StartClockTimer(timerDuration); // 타이머 재시작
    }

    public TimeSpan SetDayTimer()
    {
        DateTime tomorrowMidnight = DateTime.Today.AddDays(1);

        TimeSpan durationUntilTomorrow = tomorrowMidnight - DateTime.Now;
        timerDuration = durationUntilTomorrow.Seconds + durationUntilTomorrow.Minutes * 60 + durationUntilTomorrow.Hours * 3600;
        return durationUntilTomorrow;
    }

    public void SetTimer(TimeSpan timeSpan)
    {
        timerDuration = timeSpan.Seconds + timeSpan.Minutes * 60 + timeSpan.Hours * 3600;
    }

    public void SetTimer(int time)
    {
        timerDuration = time;
    }
}

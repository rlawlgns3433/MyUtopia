using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

public class ClockFormatTimer : MonoBehaviour
{
    private static readonly string formatTimer = "전체 갱신까지 {0:00}:{1:00}:{2:00}";
    private int timerDuration = 3600; 
    // 구글 시간으로 내일 00시 00분 00초까지의 시간
    // 네트워크 연결이 되어 있는 경우 서버 시간을 받아와야 함
    // 네트워크 연결이 되어 있지 않은 경우 기기의 시간을 받아와야 함

    public TextMeshProUGUI timerText;
    private Tween timerTween;
    private float remainingTime;
    private float startTime;

    private void Start()
    {
        GetTimerDuration();
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

        timerText.text = string.Format(formatTimer, hours, minutes, seconds);
    }

    private void OnTimerComplete()
    {
        timerText.text = string.Format(formatTimer, 0, 0, 0);
        RestartTimer();
        // 전체 단골 게시판 갱신!
    }

    void RestartTimer()
    {
        if (timerTween != null && timerTween.IsPlaying())
        {
            timerTween.Kill();
        }
        GetTimerDuration();
        StartClockTimer(timerDuration); // 타이머 재시작
    }

    private TimeSpan GetTimerDuration()
    {
        DateTime tomorrowMidnight = DateTime.Today.AddDays(1);

        TimeSpan durationUntilTomorrow = tomorrowMidnight - DateTime.Now;

        Debug.LogError($"내일까지 {timerDuration = durationUntilTomorrow.Seconds + durationUntilTomorrow.Minutes * 60 + durationUntilTomorrow.Hours * 3600}초");

        return durationUntilTomorrow;
    }

}

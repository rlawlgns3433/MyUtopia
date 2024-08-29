using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TimeData
{
    public string EnterTime { get; set; }
    public string QuitTime { get; set; }
    public string LastDaily { get; set; }
    public string LastWeekly { get; set; }
    public string LastMonthly { get; set; }
    public string FirstLogInDaily { get; set; }
}

public class UtilityTime : Singleton<UtilityTime>, ISingletonCreatable
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "TimeData.json");
    private static int seconds;
    public static int Seconds { get { return seconds; } private set { seconds = value; } }

    public static TimeData previousTimeData;

    public static bool dailyMissionReset { get; private set; }
    public static bool weeklyMissionReset { get; private set; }
    public static bool monthlyMissionReset { get; private set; }

    private static TimeSpan serverTimeOffset;
    public static bool isLoadComplete = false;
    public static bool isFirstLoginToday { get; private set; }
    private DateTime currentDate;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnApplicationStart()
    {
        GameObject obj = new GameObject("UtilityTime");
        DontDestroyOnLoad(obj);
        obj.AddComponent<UtilityTime>();
    }

    private async void Start()
    {
        //await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        await LoadPreviousTimeData();
        await CalculateElapsedTime();
        await SaveEnterTime();
        await SyncServerTime();
        CheckMissionsAvailability();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveQuitTimeSync();
        }
    }

    private void OnDestroy()
    {
        SaveQuitTimeSync();
    }

    private static async UniTask SyncServerTime()
    {
        string serverTimeString = await GetServerTimeAsync();
        if (DateTime.TryParse(serverTimeString, out DateTime serverTime))
        {
            DateTime localTime = DateTime.Now;
            serverTimeOffset = serverTime - localTime;
        }
    }

    public static DateTime GetCurrentTime()
    {
        return DateTime.UtcNow + serverTimeOffset;
    }

    public static async UniTask<string> GetServerTimeAsync()
    {
        using (UnityWebRequest req = UnityWebRequest.Get("http://google.com"))
        {
            req.timeout = 5;
            var op = await req.SendWebRequest().ToUniTask();
            if (op.result == UnityWebRequest.Result.Success)
            {
                string serverTime = req.GetResponseHeader("Date");
                if (!string.IsNullOrEmpty(serverTime))
                {
                    var localizedTime = ToLocalize(serverTime);
                    return localizedTime.ToString("o");
                }
            }
        }
        return DateTime.UtcNow.ToString("o");
    }

    private static DateTime ToLocalize(string serverTime)
    {
        if (DateTime.TryParse(serverTime, out DateTime serverDateTime))
        {
            return serverDateTime.ToLocalTime();
        }
        return DateTime.Now;
    }

    public static async UniTask SaveEnterTime()
    {
        string enterTimeString = await GetServerTimeAsync();
        previousTimeData.EnterTime = enterTimeString;
        if(previousTimeData.LastDaily == null)
        {
            previousTimeData.LastDaily = enterTimeString;
        }
        if(previousTimeData.LastWeekly == null)
        {
            previousTimeData.LastWeekly = enterTimeString;
        }
        if(previousTimeData.LastMonthly == null)
        {
            previousTimeData.LastMonthly = enterTimeString;
        }
        await SaveTimeDataAsync(previousTimeData);
    }

    private async UniTask SaveQuitTime()
    {
        var quitTime = GetCurrentTime();
        previousTimeData.QuitTime = quitTime.ToString("o");
        await SaveTimeDataAsync(previousTimeData);
    }

    public static void SaveQuitTimeSync()
    {
        var quitTime = GetCurrentTime();
        previousTimeData.QuitTime = quitTime.ToString("o");
        SaveTimeDataSync(previousTimeData);
    }

    public async UniTask CalculateElapsedTime()
    {
        if (!string.IsNullOrEmpty(previousTimeData.EnterTime) && !string.IsNullOrEmpty(previousTimeData.QuitTime))
        {
            string serverTimeString = await GetServerTimeAsync();
            DateTime serverTime = DateTime.Parse(serverTimeString);
            DateTime enterTime = DateTime.Parse(previousTimeData.EnterTime);
            DateTime quitTime = DateTime.Parse(previousTimeData.QuitTime);

            TimeSpan elapsedTime = serverTime - quitTime;
            Seconds = (int)elapsedTime.TotalSeconds;
        }
    }

    private async UniTask LoadPreviousTimeData()
    {
        previousTimeData = await LoadTimeDataAsync();
        if (previousTimeData == null)
        {
            previousTimeData = new TimeData();
        }
    }

    private static async UniTask<TimeData> LoadTimeDataAsync()
    {
        if (File.Exists(filePath))
        {
            using (var sr = new StreamReader(filePath))
            using (var jr = new JsonTextReader(sr))
            {
                var deserializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                return await UniTask.RunOnThreadPool(() => deserializer.Deserialize<TimeData>(jr));
            }
        }
        return new TimeData();
    }

    private static void SaveTimeDataSync(TimeData timeData)
    {
        using (var sw = new StreamWriter(filePath))
        using (var jw = new JsonTextWriter(sw))
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };
            serializer.Serialize(jw, timeData);
        }
    }

    private static async UniTask SaveTimeDataAsync(TimeData timeData)
    {
        using (var sw = new StreamWriter(filePath))
        using (var jw = new JsonTextWriter(sw))
        {
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };
            await UniTask.RunOnThreadPool(() => serializer.Serialize(jw, timeData));
        }
    }

    public void CheckMissionsAvailability()
    {
        currentDate = GetCurrentTime().Date;

        DateTime lastDailyDate = DateTime.Parse(previousTimeData.LastDaily).Date;
        DateTime lastWeeklyDate = DateTime.Parse(previousTimeData.LastWeekly).Date;
        DateTime lastMonthlyDate = DateTime.Parse(previousTimeData.LastMonthly).Date;

        dailyMissionReset = lastDailyDate < currentDate;
        weeklyMissionReset = lastWeeklyDate.AddDays(7) <= currentDate;
        monthlyMissionReset = lastMonthlyDate.AddMonths(1) <= currentDate;

        DateTime enterTime = DateTime.Parse(previousTimeData.EnterTime).Date;
        isFirstLoginToday = previousTimeData.FirstLogInDaily == null || enterTime > DateTime.Parse(previousTimeData.FirstLogInDaily).Date;

        if (isFirstLoginToday)
        {
            previousTimeData.FirstLogInDaily = currentDate.ToString("o");
        }

        isLoadComplete = true;
        SaveTimeDataSync(previousTimeData);
    }

    public void SetMissionData()
    {
        if (dailyMissionReset)
        {
            previousTimeData.LastDaily = currentDate.ToString("o");
            MissionManager.Instance.ResetMissions(MissionDayTypes.Daily);
        }
        else
        {
            MissionManager.Instance.LoadGameData();
        }

        if (weeklyMissionReset)
        {
            previousTimeData.LastWeekly = currentDate.ToString("o");
            MissionManager.Instance.ResetMissions(MissionDayTypes.Weekly);
        }

        if (monthlyMissionReset)
        {
            previousTimeData.LastMonthly = currentDate.ToString("o");
            MissionManager.Instance.ResetMissions(MissionDayTypes.Monthly);
        }
        else
        {
        }
        //if (!dailyMissionReset && !weeklyMissionReset && !monthlyMissionReset)
        //{
        //    MissionManager.Instance.LoadGameData();
        //}
    }

    private static bool IsDateDifferent(DateTime currentDate, DateTime lastDate, TimeSpan interval)
    {
        return currentDate.Date > lastDate.Date;
    }

    public bool ShouldBeCreatedInScene(string sceneName)
    {
        return sceneName == "SampleScene CBTJH" || sceneName == "WorldMap";
    }
}

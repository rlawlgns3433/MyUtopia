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
    public float QuitTime { get; set; }
    public string LastDaily { get; set; }
    public string LastWeekly { get; set; }
    public string LastMonthly { get; set; }
    public string FirstLogInDaily { get; set; }
}

public class UtilityTime : MonoBehaviour
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "TimeData.json");
    private static int seconds;
    public static int Seconds { get { return seconds; } private set { seconds = value; } }

    private static TimeData previousTimeData;

    public static bool dailyMissionReset { get; private set; }
    public static bool weeklyMissionReset { get; private set; }
    public static bool monthlyMissionReset { get; private set; }

    private static TimeSpan serverTimeOffset;
    public static bool isLoadComplete = false;
    public static bool isFirstLoginToday { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnApplicationStart()
    {
        GameObject obj = new GameObject("UtilityTime");
        DontDestroyOnLoad(obj);
        obj.AddComponent<UtilityTime>();
    }

    private async void Start()
    {
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

    private void OnApplicationQuit()
    {
        SaveQuitTimeSync();
    }

    private static async UniTask SyncServerTime()
    {
        string serverTimeString = await GetServerTimeAsync();
        DateTime serverTime = DateTime.Parse(serverTimeString);
        DateTime localTime = DateTime.Now;
        serverTimeOffset = serverTime - localTime;
    }

    private static DateTime GetCurrentTime()
    {
        return DateTime.Now + serverTimeOffset;
    }

    private static async UniTask<string> GetServerTimeAsync()
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

    private static async UniTask SaveEnterTime()
    {
        string enterTimeString = await GetServerTimeAsync();
        previousTimeData.EnterTime = enterTimeString;
        await SaveTimeDataAsync(previousTimeData);
    }

    private static async UniTask SaveQuitTime()
    {
        float quitTimeFloat = Time.time;
        previousTimeData.QuitTime = quitTimeFloat;
        await SaveTimeDataAsync(previousTimeData);
    }

    private static void SaveQuitTimeSync()
    {
        float quitTimeFloat = Time.time;
        previousTimeData.QuitTime = quitTimeFloat;
        SaveTimeDataSync(previousTimeData);
    }

    private static async UniTask CalculateElapsedTime()
    {
        if (!string.IsNullOrEmpty(previousTimeData.EnterTime) && previousTimeData.QuitTime > 0)
        {
            string serverTimeString = await GetServerTimeAsync();
            DateTime serverTime = DateTime.Parse(serverTimeString);
            DateTime enterTime = DateTime.Parse(previousTimeData.EnterTime);
            float quitTime = previousTimeData.QuitTime;

            DateTime quitDateTime = enterTime.AddSeconds(quitTime);

            TimeSpan elapsedTime = serverTime - quitDateTime;
            Seconds = (int)elapsedTime.TotalSeconds;
        }
    }

    private static async UniTask LoadPreviousTimeData()
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
                var deserializer = new JsonSerializer();
                deserializer.TypeNameHandling = TypeNameHandling.All;
                TimeData data = await UniTask.RunOnThreadPool(() => deserializer.Deserialize<TimeData>(jr));
                return data;
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

    private static TimeData LoadTimeDataSync()
    {
        if (File.Exists(filePath))
        {
            using (var sr = new StreamReader(filePath))
            using (var jr = new JsonTextReader(sr))
            {
                var deserializer = new JsonSerializer();
                deserializer.TypeNameHandling = TypeNameHandling.All;
                TimeData data = deserializer.Deserialize<TimeData>(jr);
                return data;
            }
        }
        return new TimeData();
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

    private void CheckMissionsAvailability()
    {
        DateTime currentDate = GetCurrentTime();

        bool wasDailyReset = dailyMissionReset;
        bool wasWeeklyReset = weeklyMissionReset;
        bool wasMonthlyReset = monthlyMissionReset;

        dailyMissionReset = previousTimeData.LastDaily == null || IsDateDifferent(currentDate, DateTime.Parse(previousTimeData.LastDaily), TimeSpan.FromDays(1));
        weeklyMissionReset = previousTimeData.LastWeekly == null || IsDateDifferent(currentDate, DateTime.Parse(previousTimeData.LastWeekly), TimeSpan.FromDays(7));
        monthlyMissionReset = previousTimeData.LastMonthly == null || IsDateDifferent(currentDate, DateTime.Parse(previousTimeData.LastMonthly), TimeSpan.FromDays(30));

        if (dailyMissionReset)
        {
            previousTimeData.LastDaily = currentDate.ToString("o");
            MissionManager.Instance.ResetMissions(MissionTypes.Daily);
            Debug.Log("Daily mission reset.");
        }
        else
        {
            Debug.Log("Daily Not reset.");
        }

        if (weeklyMissionReset)
        {
            previousTimeData.LastWeekly = currentDate.ToString("o");
            MissionManager.Instance.ResetMissions(MissionTypes.Weekly);
            Debug.Log("Weekly mission reset.");
        }
        else
        {
            Debug.Log("Weekly Not reset.");
        }

        if (monthlyMissionReset)
        {
            previousTimeData.LastMonthly = currentDate.ToString("o");
            MissionManager.Instance.ResetMissions(MissionTypes.Monthly);
            Debug.Log("Monthly mission reset.");
        }
        else
        {
            Debug.Log("Monthly Not reset.");
        }

        DateTime enterTime = DateTime.Parse(previousTimeData.EnterTime);
        isFirstLoginToday = previousTimeData.FirstLogInDaily == null || enterTime.Date > DateTime.Parse(previousTimeData.FirstLogInDaily).Date;

        if (isFirstLoginToday)
        {
            previousTimeData.FirstLogInDaily = enterTime.ToString("o");
            Debug.Log("First login today.");
        }
        else
        {
            Debug.Log("Not first login today.");
        }

        isLoadComplete = true;
        SaveTimeDataSync(previousTimeData);
    }


    private bool IsDateDifferent(DateTime currentDate, DateTime lastDate, TimeSpan interval)
    {
        DateTime currentDateAtMidnight = currentDate.Date;
        DateTime lastDateAtMidnight = lastDate.Date;

        return (currentDateAtMidnight - lastDateAtMidnight) >= interval;
    }
}

using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TimeData
{
    private string quitTime;
    public string QuitTime
    {
        get
        {
            return this.quitTime;
        }
        set
        {
            this.quitTime = value;
        }
    }
    private string enterTime;

    public string EnterTime
    {
        get
        {
            return this.enterTime;
        }
        set
        {
            this.enterTime = value;
        }
    }
}

public static class UtilityTime
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "Time.json");
    private static int seconds;
    public static int Seconds { get { return seconds; } set { seconds = value; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static async void OnApplicationStart()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                Debug.Log("인터넷 연결 x");
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                await SaveEnterTime();
                CompareStoredAndCurrentTime();
                break;
        }

        Application.quitting += OnApplicationQuit;
    }

    private static void OnApplicationQuit()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                SaveQuitTimeOffLine();
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                SaveQuitTimeOnLine().Forget();
                break;
        }
    }

    private static async UniTask<string> GetServerTimeAsync()
    {
        using (UnityWebRequest req = UnityWebRequest.Get("http://google.com"))
        {
            var op = await req.SendWebRequest();
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
        return DateTime.Now.ToString("o");
    }

    private static DateTime ToLocalize(string serverTime)
    {
        if (DateTime.TryParse(serverTime, out DateTime serverDateTime))
        {
            return serverDateTime.ToLocalTime();
        }
        else
        {
            return DateTime.Now;
        }
    }

    private static async UniTask SaveEnterTime()
    {
        string enterTimeString = await GetServerTimeAsync();
        TimeData timeData = LoadTimeData();
        timeData.EnterTime = enterTimeString;
        SaveTimeData(timeData);
    }

    private static async UniTask SaveQuitTimeOnLine()
    {
        string quitTimeString = await GetServerTimeAsync();
        TimeData timeData = LoadTimeData();
        timeData.QuitTime = quitTimeString;
        SaveTimeData(timeData);
    }

    private static void SaveQuitTimeOffLine()
    {
        string quitTimeString = DateTime.Now.ToString("o") + "a";
        TimeData timeData = LoadTimeData();
        timeData.QuitTime = quitTimeString;
        SaveTimeData(timeData);
    }

    private static void CompareStoredAndCurrentTime()
    {
        if (File.Exists(filePath))
        {
            TimeData data = LoadTimeData();

            if (!string.IsNullOrEmpty(data.QuitTime) && !string.IsNullOrEmpty(data.EnterTime))
            {
                DateTime quitTime = DateTime.Parse(data.QuitTime);
                DateTime enterTime = DateTime.Parse(data.EnterTime);
                TimeSpan compareTime = enterTime - quitTime;
                Seconds = (int)compareTime.TotalSeconds;
                Debug.Log($"Seconds since last quit: {Seconds}");
            }
        }
        else
        {
            Debug.Log("No quit time found.");
        }
    }

    private static TimeData LoadTimeData()
    {
        if (File.Exists(filePath))
        {
            using (var jr = new JsonTextReader(new StreamReader(filePath)))
            {
                var deserializer = new JsonSerializer();
                deserializer.TypeNameHandling = TypeNameHandling.All;
                return deserializer.Deserialize<TimeData>(jr);
            }
        }
        return new TimeData();
    }

    private static void SaveTimeData(TimeData timeData)
    {
        using (var jw = new JsonTextWriter(new StreamWriter(filePath)))
        {
            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Serialize(jw, timeData);
        }
    }
}

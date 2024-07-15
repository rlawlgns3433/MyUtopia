using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TimeData
{
    public string EnterTime { get; set; }
    public float QuitTime { get; set; }
}

public class UtilityTime : MonoBehaviour
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "TimeData.json");
    private static int seconds;
    public static int Seconds { get { return seconds; } private set { seconds = value; } }

    private static TimeData previousTimeData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnApplicationStart()
    {
        GameObject obj = new GameObject("UtilityTime");
        DontDestroyOnLoad(obj);
        obj.AddComponent<UtilityTime>();
        Application.wantsToQuit += OnApplicationWantsToQuit;
    }

    private async void Start()
    {
        Debug.Log($"File path: {filePath}");
        await LoadPreviousTimeData();
        await CalculateElapsedTime();
        await SaveEnterTime();
    }

    private static bool OnApplicationWantsToQuit()
    {
        Debug.Log("OnApplicationWantsToQuit called.");
        HandleQuit();
        return true;
    }

    private static void HandleQuit()
    {
        Debug.Log("HandleQuit called.");
        SaveQuitTime();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private static async Task<string> GetServerTimeAsync()
    {
        using (UnityWebRequest req = UnityWebRequest.Get("http://google.com"))
        {
            req.timeout = 5;
            var op = await req.SendWebRequest().ToUniTask();
            if (op.result == UnityWebRequest.Result.Success)
            {
                string serverTime = req.GetResponseHeader("Date");
                Debug.Log($"Server time received: {serverTime}");
                if (!string.IsNullOrEmpty(serverTime))
                {
                    var localizedTime = ToLocalize(serverTime);
                    Debug.Log($"Localized server time: {localizedTime}");
                    return localizedTime.ToString("o");
                }
                else
                {
                    Debug.LogError("Server time is null or empty.");
                }
            }
            else
            {
                Debug.LogError($"Failed to get server time. Error: {req.error}");
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

    private static async Task SaveEnterTime()
    {
        string enterTimeString = await GetServerTimeAsync();
        Debug.Log($"Enter time: {enterTimeString}");
        previousTimeData.EnterTime = enterTimeString;
        SaveTimeData(previousTimeData);

        TimeData timeData = LoadTimeData();
        Debug.Log($"Loaded Enter time after save: {timeData.EnterTime}");
    }

    private static void SaveQuitTime()
    {
        Debug.Log("SaveQuitTime called.");
        float quitTimeFloat = Time.time;
        Debug.Log($"Quit time (Time.time): {quitTimeFloat}");
        previousTimeData.QuitTime = quitTimeFloat;
        SaveTimeData(previousTimeData);

        TimeData timeData = LoadTimeData();
        Debug.Log($"Loaded Quit time after save: {timeData.QuitTime}");
    }

    private static async Task CalculateElapsedTime()
    {
        if (!string.IsNullOrEmpty(previousTimeData.EnterTime) && previousTimeData.QuitTime > 0)
        {
            string serverTimeString = await GetServerTimeAsync();
            DateTime serverTime = DateTime.Parse(serverTimeString);
            DateTime enterTime = DateTime.Parse(previousTimeData.EnterTime);
            float quitTime = previousTimeData.QuitTime;

            DateTime quitDateTime = enterTime.AddSeconds(quitTime);
            Debug.Log($"Quit date time: {quitDateTime}");

            TimeSpan elapsedTime = serverTime - quitDateTime;
            Seconds = (int)elapsedTime.TotalSeconds;
            Debug.Log($"Seconds since last quit: {Seconds}");
        }
        else
        {
            Debug.Log("No valid enter or quit time found.");
        }
    }

    private static async Task LoadPreviousTimeData()
    {
        previousTimeData = LoadTimeData();
        if (previousTimeData == null)
        {
            previousTimeData = new TimeData();
        }
    }

    private static TimeData LoadTimeData()
    {
        if (File.Exists(filePath))
        {
            using (var sr = new StreamReader(filePath))
            using (var jr = new JsonTextReader(sr))
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
}

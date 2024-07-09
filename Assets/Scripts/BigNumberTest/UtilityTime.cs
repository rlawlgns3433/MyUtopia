using Cysharp.Threading.Tasks;
using DG.Tweening.Plugins.Core.PathCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using TMPro;
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
            this .enterTime = value;
        }
    }
}

public class UtilityTime : MonoBehaviour
{
    private string filePath;
    private int seconds;
    public int Seconds { get { return seconds; } set { this.seconds = value; } }

    private async void Start()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                Debug.Log("인터넷 연결 x");
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                filePath = System.IO.Path.Combine(Application.persistentDataPath, "Time.json");
                await SaveEnterTime();
                CompareStoredAndCurrentTime();
                break;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                filePath = System.IO.Path.Combine(Application.persistentDataPath, "Time.json");
                await SaveEnterTime();
                CompareStoredAndCurrentTime();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                SaveQuitTimeOffLine();
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                SaveQuitTimeOnLine().Forget();
                break;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                SaveQuitTimeOnLine().Forget();
                break;
        }
    }

    private async UniTask<string> GetServerTimeAsync()
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

    private DateTime ToLocalize(string serverTime)
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

    private async UniTask SaveEnterTime()
    {
        string enterTimeString = await GetServerTimeAsync();
        TimeData timeData = LoadTimeData();
        timeData.EnterTime = enterTimeString;
        SaveTimeData(timeData);
    }

    private async UniTask SaveQuitTimeOnLine()
    {
        string quitTimeString = await GetServerTimeAsync();
        TimeData timeData = LoadTimeData();
        timeData.QuitTime = quitTimeString;
        SaveTimeData(timeData);
    }

    private void SaveQuitTimeOffLine()
    {
        string quitTimeString = DateTime.Now.ToString("o") + "a";
        TimeData timeData = LoadTimeData();
        timeData.QuitTime = quitTimeString;
        SaveTimeData(timeData);
    }

    private void CompareStoredAndCurrentTime()
    {
        if (File.Exists(filePath))
        {
            TimeData data = LoadTimeData();

            if (!string.IsNullOrEmpty(data.QuitTime) && !string.IsNullOrEmpty(data.EnterTime))
            {
                DateTime quitTime = DateTime.Parse(data.QuitTime);
                DateTime enterTime = DateTime.Parse(data.EnterTime);
                TimeSpan compareTime = enterTime - quitTime;
                Seconds = (int)compareTime.TotalSeconds; // test(시간별 획득재화)
                //BigNum bigNumSeconds = new BigNum(seconds.ToString());
                //BigNumSeconds = new BigNum(seconds.ToString());
                Debug.Log($"Seconds since last quit: {Seconds}");
                //Debug.Log($"BigNum format: {bigNumSeconds}");

                //if (timeText != null)
                //{
                //    timeText.text = bigNumSeconds.ToString();
                //}
            }
        }
        else
        {
            Debug.Log("No quit time found.");
        }
    }

    private TimeData LoadTimeData()
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

    private void SaveTimeData(TimeData timeData)
    {
        var QuitTimeConverter = new QuitTimeConverter();
        using (var jw = new JsonTextWriter(new StreamWriter(filePath)))
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(QuitTimeConverter);
            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Serialize(jw, timeData);
        }
    }
}

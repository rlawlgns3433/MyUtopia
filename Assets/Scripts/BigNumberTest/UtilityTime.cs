using Cysharp.Threading.Tasks;
using DG.Tweening.Plugins.Core.PathCore;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TimeData
{
    public string quitTime { get; set; }
}

public class UtilityTime : MonoBehaviour
{
    public TextMeshPro timeText;
    private string filePath;

    private void Start()
    {
        filePath = System.IO.Path.Combine(Application.persistentDataPath, "quitTime.json");
        CompareStoredAndCurrentTime();
        GetWaitWebRequest();
    }

    private void OnApplicationQuit()
    {
        SaveQuitTime();
    }

    async UniTask<string> GetTextAsync(UnityWebRequest req)
    {
        var op = await req.SendWebRequest();
        if (op.result == UnityWebRequest.Result.Success)
        {
            string serverTime = op.GetResponseHeader("Date");
            if (!string.IsNullOrEmpty(serverTime))
            {
                var localizedTime = ToLocalize(serverTime);
            }
        }
        return op.downloadHandler.text;
    }

    async UniTaskVoid WaitWebRequestAsync()
    {
        try
        {
            var req = GetTextAsync(UnityWebRequest.Get("http://google.com"));
            string result = await req;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching text: {e.Message}");
        }
    }

    private DateTime ToLocalize(string serverTime)
    {
        DateTime serverDateTime;
        if (DateTime.TryParse(serverTime, out serverDateTime))
        {
            return serverDateTime.ToLocalTime();
        }
        else
        {
            return DateTime.Now;
        }
    }
    public void GetWaitWebRequest()
    {
        WaitWebRequestAsync().Forget();
    }

    private void SaveQuitTime()
    {
        DateTime quitTime = DateTime.Now;
        TimeData timeData = new TimeData { quitTime = quitTime.ToString("o") };
        //string json = JsonUtility.ToJson(timeData);
        //File.WriteAllText(filePath, json);
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

    private void CompareStoredAndCurrentTime()
    {
        if (File.Exists(filePath))
        {
            //string json = File.ReadAllText(filePath);
            //TimeData timeData = JsonUtility.FromJson<TimeData>(json);
            //DateTime quitTime = DateTime.Parse(timeData.quitTime);
            DateTime currentTime = DateTime.Now;

            TimeData data = null;
            using (var jr = new JsonTextReader(new StreamReader(filePath)))
            {
                var deserializer = new JsonSerializer();
                deserializer.TypeNameHandling = TypeNameHandling.All;
                data = deserializer.Deserialize<TimeData>(jr);
            }
            DateTime quitTime = DateTime.Parse(data.quitTime);
            TimeSpan compareTime = currentTime - quitTime;
            var seconds = (int)compareTime.TotalSeconds * 100; // test(Ω√∞£∫∞ »πµÊ¿Á»≠)
            BigNum bigNumSeconds = new BigNum(seconds.ToString());

            Debug.Log(filePath);
            Debug.Log($"Seconds since last quit: {seconds}");
            Debug.Log($"BigNum format: {bigNumSeconds}");

            if (timeText != null)
            {
                timeText.text = bigNumSeconds.ToString();
            }
        }
        else
        {
            Debug.Log("No quit time found.");
        }
    }
}

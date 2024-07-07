using Cysharp.Threading.Tasks;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TimeData
{
    public string quitTime;
}

public class UtilityTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "quitTime.json");
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
                UpdateTimeText(localizedTime);
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

    private void UpdateTimeText(DateTime localizedTime)
    {
        if (timeText != null)
        {
            timeText.text = localizedTime.ToString("yyyy-MM-dd HH:mm:ss");
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
        string json = JsonUtility.ToJson(timeData);
        File.WriteAllText(filePath, json);
    }

    private void CompareStoredAndCurrentTime()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            TimeData timeData = JsonUtility.FromJson<TimeData>(json);
            DateTime quitTime = DateTime.Parse(timeData.quitTime);
            DateTime currentTime = DateTime.Now;

            TimeSpan compareTime = currentTime - quitTime;
            Debug.Log(filePath);
            Debug.Log($"Seconds since last quit: {compareTime.TotalSeconds}");
        }
    }
}

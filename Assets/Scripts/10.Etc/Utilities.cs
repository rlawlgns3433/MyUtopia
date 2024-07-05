using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Utilities : MonoBehaviour
{
    private void Start()
    {
        GetWaitWebRequest();
    }

    async UniTask<string> GetTextAsync(UnityWebRequest req)
    {
        var op = await req.SendWebRequest();
        if (op.result == UnityWebRequest.Result.Success)
        {
            string serverTime = op.GetResponseHeader("Date");
            var localizedTime = ToLocalize(serverTime);
            Debug.Log($"Server Time: {localizedTime}");
        }
        return op.downloadHandler.text;
    }

    async UniTaskVoid WaitWebRequestAsync()
    {
        try
        {
            var req = GetTextAsync(UnityWebRequest.Get("http://google.com"));
            string result = await req;
            Debug.Log($"{result}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching text: {e.Message}");
        }
    }

    private DateTime ToLocalize(string serverTime)
    {
        DateTime serverDateTime = DateTime.Parse(serverTime);
        return serverDateTime.ToLocalTime();
    }

    public void GetWaitWebRequest()
    {
        WaitWebRequestAsync().Forget();
    }
}

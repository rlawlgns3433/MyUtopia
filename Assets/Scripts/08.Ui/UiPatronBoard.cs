using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UiPatronBoard : MonoBehaviour, IUISetupable, IGrowable
{
    public List<UiRequestInfo> requests = new List<UiRequestInfo>();
    public UiRequestInfo requestPrefab;
    public TextMeshProUGUI textRefreshTimer;
    public TextMeshProUGUI textMax;
    public Transform requestParent;
    public PatronBoard patronBoard;
    private StorageProduct storageProduct;
    public StorageProduct StorageProduct
    {
        get
        {
            if(storageProduct == null)
            {
                storageProduct = FloorManager.Instance.GetFloor("B3").storage as StorageProduct;
            }
            return storageProduct;
        }
    }
    public ClockFormatTimer clockFormatTimer;
    public bool IsUpgrading { get => patronBoard.IsUpgrading; set => patronBoard.IsUpgrading = value; }
    public double UpgradeTimeLeft 
    {
        get => patronBoard.BuildingStat.UpgradeTimeLeft;
        set => patronBoard.BuildingStat.UpgradeTimeLeft = value;
    }

    private void Start()
    {
        clockFormatTimer.SetDayTimer();
        clockFormatTimer.canStartTimer = true;
        clockFormatTimer.StartClockTimer();
        SetFinishUi();
    }

    public void FinishUpgrade()
    {
        clockFormatTimer.timerText.gameObject.SetActive(false);
    }

    public void SetFinishUi()
    {
        if(clockFormatTimer.remainingTime <= 0)
        {
            ResetTimer();
            patronBoard.ResetTimer();
        }

        SetRequests();

        if(patronBoard.BuildingStat.Level >= patronBoard.BuildingStat.Level_Max)
        {
            textMax.gameObject.SetActive(true);
        }
        else
        {
            textMax.gameObject.SetActive(false);
        }
    }

    public void SetRequests()
    {
        var loadRequests = LoadRequests(patronBoard.BuildingStat.Level);

        foreach (var request in loadRequests)
        {
            var requestInfo = Instantiate(requestPrefab, requestParent);
            var exchange = new ExchangeStat(request);
            requestInfo.SetData(exchange);
            patronBoard.exchangeStats.Add(exchange);
            requests.Add(requestInfo);
        }
    }

    public List<int> LoadRequests(int level)
    {
        return patronBoard.requests;
    }

    public void Refresh()
    {
        foreach(var request in requests)
        {
            request.Refresh();
        }
    }

    public void LevelUp()
    {
        patronBoard.LevelUp();
    }

    private void ResetTimer()
    {
        clockFormatTimer.RestartTimer();
    }

    public void SetStartUi()
    {
        clockFormatTimer.timerText.gameObject.SetActive(true);
    }
}

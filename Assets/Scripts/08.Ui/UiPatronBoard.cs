using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UiPatronBoard : MonoBehaviour, IUISetupable, IGrowable
{
    public List<UiRequestInfo> requests = new List<UiRequestInfo>();
    public UiRequestInfo requestPrefab;
    public TextMeshProUGUI textRefreshTimer;
    public Transform requestParent;
    public Building building;
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
    public bool IsUpgrading { get => building.IsUpgrading; set => building.IsUpgrading = value; }

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
        SetRequests();
        //ResetTimer();
    }

    public void SetRequests()
    {
        var loadRequests = LoadRequests(building.BuildingStat.Level);

        foreach (var request in loadRequests)
        {
            var requestInfo = Instantiate(requestPrefab, requestParent);
            requestInfo.SetData(new ExchangeStat(request.Exchange_ID));
            requests.Add(requestInfo);
        }
    }

    public List<ExchangeData> LoadRequests(int level)
    {
        var requests = (from request in DataTableMgr.GetExchangeTable().GetKeyValuePairs.Values
                                      where request.Exchange_Level <= level
                                      select request).ToList();

        return requests;
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
        building.LevelUp();
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

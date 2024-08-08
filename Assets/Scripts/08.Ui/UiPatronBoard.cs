using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UiPatronBoard : MonoBehaviour
{
    public List<UiRequestInfo> requests = new List<UiRequestInfo>();
    public UiRequestInfo requestPrefab;
    public TextMeshProUGUI textRefreshTimer;
    public Transform requestParent;
    public Furniture furniture;
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

    public void OnEnable()
    {
        foreach(var request in requests)
        {
            Destroy(request.gameObject);
        }

        requests.Clear();

        SetRequests();
    }

    public void SetRequests()
    {
        var loadRequests = LoadRequests(furniture.FurnitureStat.Level);

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
}

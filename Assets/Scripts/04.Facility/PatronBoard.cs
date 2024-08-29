using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PatronBoard : Building
{
    public bool isSaveFileLoaded = false;
    public List<int> requests = new List<int>();
    public List<ExchangeStat> exchangeStats = new List<ExchangeStat>();

    private async void OnEnable()
    {
        await UniTask.WaitUntil(() => GameManager.Instance.isLoadedWorld);

        if(!isSaveFileLoaded)
        {
            SetRequests();
        }
        // 시간 조건
    }

    protected override void Start()
    {
        base.Start();
        clickEvent += UiManager.Instance.ShowPatronUi;
    }

    public void SetRequests()
    {
        requests = LoadRequests(BuildingStat.Level);
    }

    public List<int> LoadRequests(int level)
    {
        var requests = (from request in DataTableMgr.GetExchangeTable().GetKeyValuePairs.Values
                        where request.Exchange_Level <= level
                        select request.Exchange_ID).ToList();

        foreach(var request in requests)
        {
            var exchange = new ExchangeStat(request);
            exchangeStats.Add(exchange);
        }

        return requests;
    }

    public void ResetTimer()
    {
        exchangeStats.Clear();
        isSaveFileLoaded = false;
    }
}

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiRequestInfo : MonoBehaviour
{
    private List<UiItemInfo> itemInfos = new List<UiItemInfo>();

    public UiItemInfo itemInfoPrefab;
    public Transform itemInfoParent;
    public Button buttonReward;
    public Button buttonRefresh;

    // 교환 테이블 파싱 후 데이터 불러오기
    private ExchangeStat exchangeStat;
    public UiUpgradeCurrency rewardCoin;

    private void OnEnable()
    {
        UiManager.Instance.patronBoardUi.Refresh();
    }

    public void SetData(ExchangeStat exchangeStat)
    {
        if (exchangeStat == null)
            return;

        this.exchangeStat = exchangeStat;
        for(int i = 0; i < exchangeStat.RequireCount; ++i)
        {
            switch (exchangeStat.requireInfos[i].Type)
            {
                case 1:
                    Debug.LogError("ResourceStat: " + exchangeStat.requireInfos[i].ID + " Value: " + exchangeStat.requireInfos[i].Value);
                    AddItem(new ResourceStat(exchangeStat.requireInfos[i].ID), exchangeStat.requireInfos[i].Value);
                    break;
                case 2:
                    Debug.LogError("ItemStat: " + exchangeStat.requireInfos[i].ID + " Value: " + exchangeStat.requireInfos[i].Value);
                    AddItem(new ItemStat(exchangeStat.requireInfos[i].ID), exchangeStat.requireInfos[i].Value);
                    break;
            }

            int count = 0;
            BigNumber price = BigNumber.Zero;
            for(int j = 0; j < exchangeStat.RequireCount; ++j)
            {
                count += int.Parse(exchangeStat.requireInfos[j].Value);
                price += new BigNumber(DataTableMgr.GetItemTable().Get(exchangeStat.requireInfos[j].ID).Sell_Price);
            }

            rewardCoin.SetCurrency(price);
        }
    }

    public void OnClickReward()
    {
        if (exchangeStat == null)
            return;

        foreach(var itemInfo in itemInfos)
        {
            if (!itemInfo.IsCompleted)
                return;
        }

        // 아이템 또는 재화 감소
        foreach(var itemInfo in itemInfos)
        {
            if (itemInfo.itemStat != null)
            {
                UiManager.Instance.patronBoardUi.StorageProduct.DecreaseProduct(itemInfo.itemStat.Item_ID, itemInfo.requireCountInt);
            }
            else if (itemInfo.resourceStat != null)
            {
                CurrencyManager.currency[(CurrencyType)itemInfo.resourceStat.Resource_ID] -= itemInfo.requireCount;
            }
        }

        UiManager.Instance.patronBoardUi.Refresh();

        foreach(var exchange in UiManager.Instance.patronBoardUi.patronBoard.exchangeStats)
        {
            if(exchange.Exchange_ID == exchangeStat.Exchange_ID)
            {
                Debug.Log($"Stat true");
                exchange.IsCompleted = true;
                break;
            }
        }

        // 보상 지급
        var rewardStat = new RewardStat(exchangeStat.Reward_ID);
        if (rewardStat == null)
            return;

        for(int i = 0; i < rewardStat.RequireCount; ++i)
        {
            switch (rewardStat.requireInfos[i].Type)
            {
                case 1:
                    CurrencyManager.currency[(CurrencyType)rewardStat.requireInfos[i].Id] += rewardStat.requireInfos[i].Value.ToBigNumber();
                    break;
                case 2:
                    UiManager.Instance.patronBoardUi.StorageProduct.IncreaseProduct(rewardStat.requireInfos[i].Id, rewardStat.requireInfos[i].Value.ToBigNumber().ToInt());
                    break;
            }
        }

        UiManager.Instance.patronBoardUi.requests.Remove(this);
        SoundManager.Instance.OnClickButton(SoundType.Delivering);
        transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InOutQuad).OnComplete(() => Destroy(gameObject));
        //Destroy(gameObject);
    }

    public void AddItem(ResourceStat resourceStat, string requireCount)
    {
        var itemInfo = Instantiate(itemInfoPrefab, itemInfoParent);
        itemInfo.SetData(resourceStat, CurrencyManager.currency[(CurrencyType)resourceStat.Resource_ID], requireCount);
        itemInfos.Add(itemInfo);
    }

    public void AddItem(ItemStat itemStat, string requireCount)
    {
        var itemInfo = Instantiate(itemInfoPrefab, itemInfoParent);
        itemInfo.SetData(itemStat, new BigNumber(UiManager.Instance.patronBoardUi.StorageProduct.Products[itemStat.Item_ID]), requireCount); // 부모 클래스에서 스토리지를 가져옴
        itemInfos.Add(itemInfo);
    }

    public void Refresh()
    {
        foreach (var itemInfo in itemInfos)
        {
            if (itemInfo.itemStat != null)
            {
                itemInfo.SetData(itemInfo.itemStat, new BigNumber(UiManager.Instance.patronBoardUi.StorageProduct.Products[itemInfo.itemStat.Item_ID]), itemInfo.requireCount.ToSimpleString());

            }
            else if (itemInfo.resourceStat != null)
            {
                itemInfo.SetData(itemInfo.resourceStat, CurrencyManager.currency[(CurrencyType)itemInfo.resourceStat.Resource_ID], itemInfo.requireCount.ToSimpleString());
            }
        }
    }
}

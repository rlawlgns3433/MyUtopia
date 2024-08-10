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

    // ��ȯ ���̺� �Ľ� �� ������ �ҷ�����
    private ExchangeStat exchangeStat;

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
                    AddItem(new ResourceStat(exchangeStat.requireInfos[i].ID), exchangeStat.requireInfos[i].Value);
                    break;
                case 2:
                    AddItem(new ItemStat(exchangeStat.requireInfos[i].ID), exchangeStat.requireInfos[i].Value);
                    break;
            }
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

        // ������ �Ǵ� ��ȭ ����
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

        // ���� ����
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
        Destroy(gameObject);
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
        itemInfo.SetData(itemStat, new BigNumber(UiManager.Instance.patronBoardUi.StorageProduct.products[itemStat.Item_ID]), requireCount); // �θ� Ŭ�������� ���丮���� ������
        itemInfos.Add(itemInfo);
    }

    public void Refresh()
    {
        foreach (var itemInfo in itemInfos)
        {
            if (itemInfo.itemStat != null)
            {
                itemInfo.SetData(itemInfo.itemStat, new BigNumber(UiManager.Instance.patronBoardUi.StorageProduct.products[itemInfo.itemStat.Item_ID]), itemInfo.requireCount.ToSimpleString());

            }
            else if (itemInfo.resourceStat != null)
            {
                itemInfo.SetData(itemInfo.resourceStat, CurrencyManager.currency[(CurrencyType)itemInfo.resourceStat.Resource_ID], itemInfo.requireCount.ToSimpleString());
            }
        }
    }
}

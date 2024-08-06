using System.Collections;
using System.Collections.Generic;
using System.Resources;
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
            // Ÿ���� 1�̸� �ڿ� 2�̸� ������
            // ���̵�� �ڿ� �Ǵ� ������ �ҷ�����
            // ��ȯ �䱸��
            // �����ϱ�
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

        // ���� ����
        //DataTableMgr.GetRewardTable().Get(exchangeStat.Reward_ID);
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
}

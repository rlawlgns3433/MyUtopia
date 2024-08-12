using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageProduct : Storage
{
    public Dictionary<int, int> products = new Dictionary<int, int>();
    public bool IsFull
    {
        get
        {
            return Count >= BuildingStat.Effect_Value;
        }
    }
    public int Count
    {
        get
        {
            int count = 0;
            foreach(var kv in products)
            {
                count += kv.Value;
            }

            return count;
        }
    }

    private async void Start()
    {
        await UniWaitItemTable();

        foreach(var item in DataTableMgr.GetItemTable().GetKeyValuePairs.Values)
        {
            products.Add(item.Item_ID, 0);
        }
    }

    public void IncreaseProduct(int id , int count = 1)
    {
        if (!products.ContainsKey(id))
            products.Add(id, 0);

        products[id] += count;
    }

    public void DecreaseProduct(int id, int count = 1)
    {
        if (!products.ContainsKey(id))
            return;

        products[id] -= count;
    }

    public async UniTask UniWaitItemTable()
    {
        while (!DataTableMgr.GetAnimalTable().IsLoaded)
        {
            await UniTask.Yield();
        }
    }
}

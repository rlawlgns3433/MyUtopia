using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageProduct : Storage
{
    public Dictionary<int, int> products;
    public Dictionary<int, int> Products
    {
        get
        {
            if(products == null)
            {
                products = new Dictionary<int, int>();
                foreach (var item in DataTableMgr.GetItemTable().GetKeyValuePairs.Values)
                {
                    products.Add(item.Item_ID, 0);
                }
            }
            return products;
        }
    }
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
            foreach(var kv in Products)
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
            if(!Products.ContainsKey(item.Item_ID))
                Products.Add(item.Item_ID, 0);
        }
    }

    public void IncreaseProduct(int id , int count = 1)
    {
        if (!Products.ContainsKey(id))
            Products.Add(id, 0);

        Products[id] += count;
    }

    public void DecreaseProduct(int id, int count = 1)
    {
        if (!Products.ContainsKey(id))
            return;

        Products[id] -= count;
    }

    public async UniTask UniWaitItemTable()
    {
        while (!DataTableMgr.GetAnimalTable().IsLoaded)
        {
            await UniTask.Yield();
        }
    }

    public void SetEmpty()
    {
        Products.Clear();

        foreach (var item in DataTableMgr.GetItemTable().GetKeyValuePairs.Values)
        {
            Products.Add(item.Item_ID, 0);
        }
    }
}

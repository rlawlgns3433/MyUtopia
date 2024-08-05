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
            return Count >= FurnitureStat.Effect_Value;
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

    public void IncreaseProduct(int id , int count = 1)
    {
        if (!products.ContainsKey(id))
            products.Add(id, 0);

        products[id] += count;
    }

    public void DecreaseProduct(int id)
    {
        if (!products.ContainsKey(id))
            return;

        products[id]--;
    }
}

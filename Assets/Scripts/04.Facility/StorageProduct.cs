using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageProduct : Storage
{
    public Dictionary<int, int> products = new Dictionary<int, int>();

    public void IncreaseProduct(int id)
    {
        if (!products.ContainsKey(id))
            products.Add(id, 0);

        products[id]++;
    }

    public void DecreaseProduct(int id)
    {
        if (!products.ContainsKey(id))
            return;

        products[id]--;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiProducts : MonoBehaviour
{
    public List<UiProductSlot> uiProducts = new List<UiProductSlot>();
    public UiProductSlot uiProductPrefab;
    public Transform parent;
    public int capacity = 0;

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDisable()
    {
        for (int i = 0; i < uiProducts.Count; i++)
        {
            Destroy(uiProducts[i].gameObject);
        }
        uiProducts.Clear();
    }

    public void SetCapacity(int capacity)
    {
        this.capacity = capacity;
    }

    public void Refresh()
    {
        for (int i = 0; i < capacity; i++)
        {
            var uiProduct = Instantiate(uiProductPrefab, parent);
            uiProduct.ClearData();
            uiProducts.Add(uiProduct);
        }

        //int size = CurrencyManager.currency[CurrencyType.Craft].ToInt() > capacity ? capacity : CurrencyManager.currency[CurrencyType.Craft].ToInt();

        int size = 0;
        var kvProducts = (FloorManager.Instance.GetFloor("B3").storage as StorageProduct).Products;
        foreach (var kv in kvProducts)
        {
            size += kv.Value;
        }

        size = size > capacity ? capacity : size;

        // 종류별로 아이템

        int currentCount = 0;

        foreach (var kv in kvProducts)
        {
            for (int i = currentCount; i < kv.Value + currentCount; i++)
            {
                if(i >= capacity)
                    break;

                uiProducts[i].SetData(new ItemStat(kv.Key));
            }
            currentCount+= kv.Value;
        }

        //for (int i = 0; i < size; ++i)
        //{
        //    uiProducts[i].SetData(new ItemStat(801101));
        //}
    }
}

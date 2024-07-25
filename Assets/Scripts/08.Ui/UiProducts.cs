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

        int size = CurrencyManager.currency[CurrencyType.Craft].ToInt() > capacity ? capacity : CurrencyManager.currency[CurrencyType.Craft].ToInt();

        for (int i = 0; i < size; ++i)
        {
            uiProducts[i].SetData(new ItemStat(801101));
        }
    }
}

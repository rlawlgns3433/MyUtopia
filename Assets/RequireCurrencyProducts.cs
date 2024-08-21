using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireCurrencyProducts : MonoBehaviour
{
    public List<UiRequireCurrencyProduct> uiRequireCurrencyProducts = new List<UiRequireCurrencyProduct>();
    public UiRequireCurrencyProduct uiRequireCurrencyProduct;
    public Transform parent;
    public void SetData(RecipeStat recipeStat)
    {
        foreach(var resources in recipeStat.Resources)
        {
            if (resources.Key == 0)
                continue;
            var slot = Instantiate(uiRequireCurrencyProduct, parent);
            uiRequireCurrencyProducts.Add(slot);

            slot.SetData(resources.Key, resources.Value);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiProducts : MonoBehaviour
{
    public List<GameObject> uiProducts = new List<GameObject>();
    public GameObject uiProductPrefab;
    public Transform parent;
    public int capacity = 0;

    private void OnEnable()
    {
        for (int i = 0; i < capacity; i++)
        {
            var uiProduct = Instantiate(uiProductPrefab, parent);
            uiProducts.Add(uiProduct);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < uiProducts.Count; i++)
        {
            Destroy(uiProducts[i]);
        }
        uiProducts.Clear();
    }

    public void SetCapacity(int capacity)
    {
        this.capacity = capacity;
    }
}

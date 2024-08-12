using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCatalogue : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    private bool isCreate = false;
    private List<AnimalData> animalList = new List<AnimalData>();
    private List<CatalogueSlot> catalogueSlots = new List<CatalogueSlot>();

    private void OnEnable()
    {
        if(!isCreate)
        {
            animalList = CatalogueManager.Instance.AnimalDataList;
            CreateCatalogue();
        }
        if(catalogueSlots.Count > 0)
        {
            foreach(var slot in catalogueSlots)
            {
                slot.SetSprite();
            }
        }
    }

    private void CreateCatalogue()
    {
        for (int i = 1; i <= CatalogueManager.Instance.count; ++i)
        {
            var slot = Instantiate(prefab, parent);
            var animalSlot = slot.GetComponent<CatalogueSlot>();
            foreach(var animal in animalList)
            {
                if(animal.Animal_Type == i)
                {
                    animalSlot.AddAnimalData(animal);
                    catalogueSlots.Add(animalSlot);
                    Debug.Log($"slot{i}data = {animal.Animal_ID} / {animal.Profile}");
                }
            }
            animalSlot.SetSprite();
        }
        isCreate = true;
    }
}

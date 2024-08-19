using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CatalogueSlot : MonoBehaviour
{
    public TextMeshProUGUI typeText;
    public Image[] slot;
    public List<AnimalData> animals = new List<AnimalData>();
    public void SetData(int index)
    {
        for(int i = 0; i < slot.Length; i++)
        {
            var profile = slot[i].GetComponentInChildren<Image>();
            //profile.sprite = CatalogueManager.Instance.animalTable.Get();
        }
    }

    public void AddAnimalData(AnimalData data)
    {
        animals.Add(data);
    }

    public async void SetSprite()
    {
        for(int i = 0; i < slot.Length;i++)
        {
            if (CatalogueManager.Instance.CheckAnimal(animals[i].Animal_ID))
            {
                var profile = slot[i].GetComponentInChildren<Image>();
                profile.sprite = await animals[i].GetProfile();
            }
        }
    }

    public void SetText()
    {
        typeText.text = animals[0].GetTypeText();
    }
}

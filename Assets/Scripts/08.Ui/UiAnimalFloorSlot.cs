using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimalFloorSlot : Observer
{
    public Image imagePortrait;
    public Image imageExchange;
    public Image imageUnplace;
    public Slider sliderStamina;
    public TextMeshProUGUI textLevel;
    public AnimalData animalData;
    public AnimalClick animalClick;

    public bool IsEmpty { get; set; }
    public bool IsClicked { get; set; }

    public virtual async void SetData(AnimalClick animClick)
    {
        if (animClick == null)
            return;
        animalClick = animClick;
        animalData = animClick.AnimalWork.Animal.animalStat.AnimalData;
        Debug.Log($"UiAnimalFloorSlot : {animClick.GetInstanceID()}");

        if (imagePortrait == null)
            return;

        imagePortrait.sprite = await animalData.GetProfile();

        textLevel.text = animalData.Level.ToString();
        sliderStamina.minValue = 0f;
        sliderStamina.maxValue = DataTableMgr.GetAnimalTable().Get(animalData.Animal_ID).Stamina;
        sliderStamina.value = animClick.AnimalWork.Animal.animalStat.Stamina;
    }

    public virtual void ClearData()
    {
        animalData = default(AnimalData);
        imagePortrait.sprite = null;
        sliderStamina.minValue = 0f;
        sliderStamina.maxValue = 0f;
        sliderStamina.value = 0f;
        imagePortrait.gameObject.SetActive(false);
        sliderStamina.gameObject.SetActive(false);
        IsEmpty = true;
    }
    public override void Notify(Subject subject)
    {
        if (animalClick == null)
            return;
        sliderStamina.value = animalClick.AnimalWork.Animal.animalStat.Stamina;
        Debug.Log(sliderStamina.value);
    }
}

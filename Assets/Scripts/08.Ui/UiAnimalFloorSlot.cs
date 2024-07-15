using UnityEngine;
using UnityEngine.UI;

public class UiAnimalFloorSlot : Observer
{
    public Image imagePortrait;
    public Slider sliderStamina;
    public AnimalData animalData;
    public AnimalClick animalClick;

    private void OnDestroy()
    {
        Destroy(imagePortrait.gameObject);
        Destroy(sliderStamina.gameObject);
        Destroy(gameObject);
    }

    public virtual void SetData(AnimalClick animClick)
    {
        if (animClick == null)
            return;
        animalClick = animClick;
        animalData = animClick.AnimalWork.Animal.animalStat.AnimalData;
        Debug.Log($"UiAnimalFloorSlot : {animClick.GetInstanceID()}");

        //if(animalData.GetProfile() != null)
        //    imagePortrait.sprite = animalData.GetProfile();

        sliderStamina.minValue = 0f;
        sliderStamina.maxValue = DataTableMgr.GetAnimalTable().Get(animalData.Animal_ID).Stamina;
        sliderStamina.value = animalData.Stamina;
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
    }
    public override void Notify(Subject subject)
    {
        if (animalClick == null)
            return;
        sliderStamina.value = animalClick.AnimalWork.Animal.animalStat.Stamina;
    }
}

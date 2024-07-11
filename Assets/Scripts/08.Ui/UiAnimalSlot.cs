using UnityEngine;
using UnityEngine.UI;

public class UiAnimalSlot : Observer
{
    public int SlotIndex;

    public bool IsEmpty
    {
        get
        {
            return animalData.ID == 0;
        }
    }
    [SerializeField]
    private Image imagePortrait;
    [SerializeField]
    private Slider sliderStamina;
    public AnimalClick animalClick;
    public AnimalData animalData;

    public void SetData(AnimalClick animClick)
    {
        if (animClick == null)
            return;
        animalClick = animClick;
        animalData = animClick.AnimalWork.Animal.animalData;

        //if(animalData.GetProfile() != null)
        //    imagePortrait.sprite = animalData.GetProfile();

        sliderStamina.minValue = 0f;
        sliderStamina.maxValue = DataTableMgr.GetAnimalTable().Get(animalData.ID).Stamina;
        sliderStamina.value = animClick.AnimalWork.Animal.animalData.Stamina;
    }

    public void ClearData()
    {
        animalClick = null;
        animalData = default(AnimalData);
        imagePortrait.sprite = null;
        sliderStamina.minValue = 0f;
        sliderStamina.maxValue = 0f;
        sliderStamina.value = 0f;
    }

    public void OnClick()
    {
        if (animalData.ID == 0)
        {
            Debug.Log("Empty");
        }

        UiAnimalInventory uiAnimalInventory = GetComponentInParent<UiAnimalInventory>();

        if (SlotIndex == uiAnimalInventory.currentIndex)
        {
            animalClick.IsClicked = true;
        }
    }

    public override void Notify(Subject subject)
    {
        sliderStamina.value = animalClick.AnimalWork.Animal.animalData.Stamina;
    }
}

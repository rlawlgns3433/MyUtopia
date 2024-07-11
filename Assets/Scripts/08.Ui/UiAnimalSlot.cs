using UnityEngine;
using UnityEngine.UI;

public class UiAnimalSlot : MonoBehaviour
{
    public int SlotIndex { get; set; }

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

    private void Awake()
    {
        sliderStamina.onValueChanged.AddListener(
        (float value) =>
        {
            animalClick.AnimalWork.Animal.animalData.Stamina = (int)value;
        });
    }

    public void SetData(AnimalClick animClick)
    {
        if (animClick == null)
            return;
        animalClick = animClick;
        animalData = animClick.AnimalWork.Animal.animalData;
        Debug.Log("222" + animalClick.GetInstanceID());

        //if(animalData.GetProfile() != null)
        //    imagePortrait.sprite = animalData.GetProfile();

        sliderStamina.minValue = 0f;
        sliderStamina.maxValue = DataTableMgr.GetAnimalTable().Get(animalData.ID).Stamina;
        sliderStamina.value = animalData.Stamina;
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
        Debug.Log($"Item Id : {animalData.ID}");

        if(SlotIndex != 5)
        {
            animalClick.IsClicked = true;
        }
    }
}

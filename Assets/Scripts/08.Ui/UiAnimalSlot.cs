using UnityEngine;
using UnityEngine.UI;

public class UiAnimalSlot : UiAnimalFloorSlot
{
    public int SlotIndex;

    public bool IsEmpty
    {
        get
        {
            return animalData.Animal_ID == 0;
        }
    }

    [SerializeField]
    private Image imageAdd;

    public override void SetData(AnimalClick animClick)
    {
        base.SetData(animClick);
        imageAdd.gameObject.SetActive(false);
    }

    public override void ClearData()
    {
        base.ClearData();
        animalClick = null;
        imageAdd.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        if (animalData.Animal_ID == 0)
        {
            Debug.Log("Empty");
        }

        // 동물 인벤토리 열기
    }
}

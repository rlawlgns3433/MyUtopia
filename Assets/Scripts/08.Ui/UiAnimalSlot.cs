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

    public override void SetData(AnimalClick animClick)
    {
        base.SetData(animClick);
    }

    public override void ClearData()
    {
        base.ClearData();
        animalClick = null;
        Destroy(gameObject);
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

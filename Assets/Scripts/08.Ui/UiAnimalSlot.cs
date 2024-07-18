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

    private void OnDestroy()
    {
        Destroy(gameObject);
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
        animalClick.IsClicked = true;

        FloorManager.Instance.SetFloor(animalClick.AnimalWork.Animal.animalStat.CurrentFloor);
}
}

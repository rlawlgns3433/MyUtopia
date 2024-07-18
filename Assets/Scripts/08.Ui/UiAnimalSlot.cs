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
        if (UiManager.Instance.isAnimalMove)
        // Ŭ�� ���� �� ���� ������ �̵�
        {
            var animalStat = animalClick.AnimalWork.Animal.animalStat;
            var toFloor = $"B{FloorManager.Instance.CurrentFloorIndex}";
            GameManager.Instance.GetAnimalManager().MoveAnimal(animalStat.CurrentFloor, toFloor, animalClick.AnimalWork.Animal);
            var fromUifloorAnimal = GetComponentInParent<UiFloorAnimal>();
            fromUifloorAnimal.Clear();
            UiManager.Instance.animalListUi.MoveSlot(fromUifloorAnimal, UiManager.Instance.animalListUi.parents[FloorManager.Instance.CurrentFloorIndex - 1], this);
            // ��ü �� ��������
            var floorAnimalParent = UiManager.Instance.animalListUi.parents; 
            foreach(var parent in floorAnimalParent)
            {
                parent.Refresh();
            }
        }
        else 
        //if (UiManager.Instance.isAnimalList) + �ٴ�
        {
            animalClick.IsClicked = true;
            if(animalClick != null)
            {
                FloorManager.Instance.SetFloor(animalClick.AnimalWork.Animal.animalStat.CurrentFloor);
            }
        }
    }
}

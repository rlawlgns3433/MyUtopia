using UnityEngine;

public class UiAnimalSlot : UiAnimalFloorSlot
{
    public int SlotIndex;

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
            var slots = UiManager.Instance.animalListUi.slots;

            switch (UiManager.Instance.animalListUi.mode)
            {
                case AnimalListMode.AnimalList:
                    animalClick.IsClicked = true;
                    if (animalClick != null)
                    {
                        FloorManager.Instance.SetFloor(animalClick.AnimalWork.Animal.animalStat.CurrentFloor);
                    }

                    break;
                case AnimalListMode.Exchange:
                    IsClicked = true;

                    if (slots.Count == 0)
                    {
                        slots.Add(this);
                        imagePortrait.color = Color.grey;
                        imageExchange.gameObject.SetActive(false);
                        break;
                    }

                    if (slots.Contains(this))
                        break;

                    slots.Add(this);
                    UiManager.Instance.animalListUi.ExchangeAnimal();
                    UiManager.Instance.animalListUi.SetAnimalListMode();

                    break;
                case AnimalListMode.Eliminate:
                    if(animalClick.AnimalWork.Animal.animalStat.CurrentFloor == "B1")
                    {
                        slots.Clear();
                        break;
                    }
                    imageExchange.gameObject.SetActive(false);

                    slots.Add(this);
                    UiManager.Instance.animalListUi.EliminateAnimal();
                    UiManager.Instance.animalListUi.SetAnimalListMode();
                    break;
                default:
                    break;
            }


        }
        UiManager.Instance.mainUi.animalInventoryUi.UpdateInventory(false);
        if(FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.AnimalStat)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
            }
        }
    }
}

using Spine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        // 클릭 됐을 때 현재 층으로 이동
        {
            var animalStat = animalClick.AnimalWork.Animal.animalStat;
            var toFloor = $"B{FloorManager.Instance.CurrentFloorIndex}";
            GameManager.Instance.GetAnimalManager().MoveAnimal(animalStat.CurrentFloor, toFloor, animalClick.AnimalWork.Animal);
            var fromUifloorAnimal = GetComponentInParent<UiFloorAnimal>();
            fromUifloorAnimal.Clear();
            UiManager.Instance.animalListUi.MoveSlot(fromUifloorAnimal, UiManager.Instance.animalListUi.parents[FloorManager.Instance.CurrentFloorIndex - 1], this);
            // 전체 층 리프레시
            var floorAnimalParent = UiManager.Instance.animalListUi.parents; 
            foreach(var parent in floorAnimalParent)
            {
                parent.Refresh();
            }
        }
        else 
        //if (UiManager.Instance.isAnimalList) + 바닥
        {
            var slots = UiManager.Instance.animalListUi.slots;

            switch (UiManager.Instance.animalListUi.mode)
            {
                case AnimalListMode.AnimalList:
                    animalClick.IsClicked = true;
                    if (animalClick != null)
                    {
                        FloorManager.Instance.SetFloor(animalClick.AnimalWork.Animal.animalStat.CurrentFloor);
                        Debug.Log($"slotClick{FloorManager.Instance.CurrentFloorIndex}");
                    }

                    break;
                case AnimalListMode.Exchange:
                    IsClicked = true;

                    if (slots.Count == 0)
                    {
                        slots.Add(this);
                        break;
                    }

                    if (slots.Contains(this))
                        break;

                    slots.Add(this);
                    UiManager.Instance.animalListUi.ExchangeAnimal();

                    break;
                case AnimalListMode.Eliminate:
                    if(animalClick.AnimalWork.Animal.animalStat.CurrentFloor == "B1")
                    {
                        slots.Clear();
                        break;
                    }

                    slots.Add(this);
                    UiManager.Instance.animalListUi.EliminateAnimal();
                    break;
                default:
                    break;
            }


        }
        UiManager.Instance.mainUi.animalInventoryUi.UpdateInventory(false);
        if(FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.AnimalStat)
        {
            FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
        }
    }
}

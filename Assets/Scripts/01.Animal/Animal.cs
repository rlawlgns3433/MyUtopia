using System;
using UnityEngine;

[Serializable]
public class Animal : IGrowable, IMovable
{
    public AnimalWork animalWork;
    public AnimalStat animalStat;

    public Animal() { }
    public Animal(int animalId)
    {
        if (animalStat == null)
        {
            animalStat = new AnimalStat(animalId);
        }
    }

    [SerializeField]
    private float walkSpeed;
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    [SerializeField]
    private float runSpeed;
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    [SerializeField]
    private float idleTime;
    public float IdleTime { get => idleTime; set => idleTime = value; }
    public bool IsUpgrading { get; set; }
    public double UpgradeTimeLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event Action clickEvent;

    public void LevelUp()
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;

        if (animalStat.Level == animalStat.Level_Max)
        {
            // 머지 규칙 적용 
            /*
             1. B1에 위치한 머지 후보 동물이 있다면
                1-1. 후보가 둘 이상이라면
                    스테미나가 낮은 동물을 선택
                1-2. 후보가 하나라면
                    그 동물을 선택
             2. B2 ~ B5에 있는 후보 동물 중 스테미나가 가장 낮은 동물을 선택 
                2-1. 스테미나가 낮은 동물이 둘 이상이라면
                    같은 층에 있는 동물 선택
                2-2. 스테미나가 낮은 동물이 하나라면
                    그 동물을 선택
             */

            if (CanMerge(out var target))
            {
                if (animalWork.Merge(target))
                {
                    SoundManager.Instance.OnClickButton(SoundType.MergeAnimal);
                    return;
                }
            }
            return;
        }

        var animals = FloorManager.Instance.GetFloor(animalClick.AnimalWork.Animal.animalStat.CurrentFloor).animals;

        BigNumber lvCoin = new BigNumber(animalStat.Level_Up_Coin_Value);
        if (CurrencyManager.currency[CurrencyType.Coin] < lvCoin) // 임시 코드
            return;

        float currentStamina = animalStat.Stamina / animalStat.AnimalData.Stamina;
        animalStat.AnimalData = DataTableMgr.GetAnimalTable().Get(animalStat.Animal_ID + 1);
        animalStat.Stamina = animalStat.AnimalData.Stamina * currentStamina;

        foreach (var a in animals)
        {
            if (a.animalWork.gameObject.GetInstanceID() == animalClick.gameObject.GetInstanceID())
            {
                CurrencyManager.currency[CurrencyType.Coin] -= lvCoin;
                a.animalStat = animalStat;
                SoundManager.Instance.OnClickButton(SoundType.LevelUpAnimal);
                break;
            }
        }
    }


    public void Sale()
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;
        animalWork = animalClick.AnimalWork;
        FloorManager.Instance.GetFloor(animalWork.Animal.animalStat.CurrentFloor).RemoveAnimal(this);
        //FloorManager.Instance.CheckFloorSynergy(FloorManager.Instance.GetFloor(animalWork.Animal.animalStat.CurrentFloor)); 시너지
        CurrencyManager.currency[CurrencyType.Coin] += animalStat.Sale_Coin.ToBigNumber();
        SoundManager.Instance.OnClickButton(SoundType.Selling);
    }

    public void SetAnimal()
    {
        walkSpeed = 3f;
        runSpeed = 5f;
        idleTime = 2f;
    }

    public bool CanMerge(out AnimalWork target)
    {
        target = null;

        if (animalStat.AnimalData.Animal_Grade == 5)
            return false;

        #region Rule1
        var firstFloorAnimals = FloorManager.Instance.GetFloor("B1").animals;

        foreach (var animal in firstFloorAnimals)
        {
            if (animal.animalWork.Equals(animalWork))
                continue;

            if (animal.animalWork.Animal.animalStat.Animal_ID == animalWork.Animal.animalStat.Animal_ID)
            {
                if (target == null)
                {
                    target = animal.animalWork;
                }
                else
                {
                    target = animal.animalWork.Animal.animalStat.Stamina < target.Animal.animalStat.Stamina ? animal.animalWork : target;
                }
            }
        }

        if (target != null)
            return true;
        #endregion

        #region Rule2

        foreach (var floor in FloorManager.Instance.floors)
        {
            if (floor.Key == "B1")
                continue;

            foreach (var animal in floor.Value.animals)
            {
                if (animal.animalWork.Equals(animalWork))
                    continue;

                if (animal.animalWork.Animal.animalStat.Animal_ID == animalWork.Animal.animalStat.Animal_ID)
                {
                    if (target == null)
                    {
                        target = animal.animalWork;
                    }
                    else
                    {
                        if (animal.animalWork.Animal.animalStat.Stamina < target.Animal.animalStat.Stamina)
                        {
                            target = animal.animalWork;
                        }
                        else if (animal.animalWork.Animal.animalStat.Stamina == target.Animal.animalStat.Stamina)
                        {
                            if (animal.animalWork.Animal.animalStat.CurrentFloor == target.Animal.animalStat.CurrentFloor)
                            {
                                target = animal.animalWork;
                            }
                        }
                    }
                }
            }
        }

        if (target != null)
            return true;

        return false;
        #endregion
    }
}
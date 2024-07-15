using System;
using UnityEngine;

[Serializable]
public class Animal : IGrowable, IMovable
{
    public AnimalWork animalWork;
    public AnimalStat animalStat;
    public AnimalData standardAnimalData;

    public Animal() { }
    public Animal(int animalId)
    {
        if(animalStat == null)
        {
            animalStat = new AnimalStat(animalId);
            standardAnimalData = DataTableMgr.GetAnimalTable().Get(animalId);
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

    public event Action clickEvent;

    public void LevelUp()
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;
        var animals = FloorManager.Instance.GetFloor(animalClick.AnimalWork.Animal.animalStat.CurrentFloor).animals;

        if (animalStat.Level == animalStat.Level_Max)
        {
            if (animals.Count == 1)
                return;
            foreach(var a in animals)
            {
                if (animalWork.Equals(a.animalWork))
                    continue;
                if(a.animalStat.Level == a.animalStat.Level_Max)
                {
                    if (!animalWork.Merge(a.animalWork))
                        continue;
                    else
                        return;
                }
            }
            return;
        }
        BigNumber lvCoin = new BigNumber(animalStat.Level_Up_Coin_Value);
        if (CurrencyManager.currency[CurrencyType.Coin] < lvCoin) // 임시 코드
            return;

        animalStat.AnimalData = DataTableMgr.GetAnimalTable().Get(animalStat.Animal_ID + 1);

        foreach(var a in animals)
        {
            if(a.animalWork.gameObject.GetInstanceID() == animalClick.gameObject.GetInstanceID())
            {
                CurrencyManager.currency[CurrencyType.Coin] -= lvCoin;
                a.animalStat = animalStat;
                
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
        CurrencyManager.currency[CurrencyType.Coin] += animalStat.Sale_Coin.ToBigNumber();
    }

    public void SetAnimal()
    {
        walkSpeed = 3f;
        runSpeed = 5f;
        idleTime = 2f;
    }
}
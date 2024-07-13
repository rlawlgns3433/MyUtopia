using System;
using UnityEngine;

[Serializable]
public class Animal : IGrowable, IMovable
{
    public AnimalWork animalWork;
    public AnimalData animalData;
    public Animal() { }
    public Animal(int animalId)
    {
        animalData = DataTableMgr.GetAnimalTable().Get(animalId);
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

        var animals = FloorManager.Instance.GetFloor(animalWork.currentFloor).animals;

        if (animalData.Level == animalData.Level_Max)
        {
            if (animals.Count == 1)
                return;
            foreach(var a in animals)
            {
                if (animalWork.Equals(a.animalWork))
                    continue;
                if(a.animalData.Level == a.animalData.Level_Max)
                {
                    if (!animalWork.Merge(a.animalWork))
                        continue;
                    else
                        return;
                }
            }
            return;
        }
        BigNumber lvCoin = new BigNumber(animalData.Level_Up_Coin_Value);
        if (CurrencyManager.currency[CurrencyType.Coin] < lvCoin) // 임시 코드
            return;

        animalData = DataTableMgr.GetAnimalTable().Get(animalData.Animal_ID + 1);

        foreach(var a in animals)
        {
            if(a.animalWork.gameObject.GetInstanceID() == animalClick.gameObject.GetInstanceID())
            {
                CurrencyManager.currency[CurrencyType.Coin] -= lvCoin;
                a.animalData = animalData;
                
                break;
            }
        }
    }


    public void Sale()
    {
        FloorManager.Instance.GetFloor(animalWork.currentFloor).RemoveAnimal(this);
        CurrencyManager.currency[CurrencyType.Coin] += animalData.Sale_Coin.ToBigNumber();
    }

    public void SetAnimal()
    {
        walkSpeed = 3f;
        runSpeed = 5f;
        idleTime = 2f;
    }
}
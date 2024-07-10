using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public class Animal : IGrowable, ISaleable, IConductable, IMovable
{
    public AnimalWork animalWork;
    public AnimalData animalData;
    public Animal() { }
    public Animal(Animal other)
    {
        this.costForLevelUp = other.costForLevelUp;
        this.coinForSale = other.coinForSale;
        this.stamina = other.stamina;
        this.workload = other.workload;
        this.walkSpeed = other.walkSpeed;
        this.runSpeed = other.runSpeed;
        this.idleTime = other.idleTime;
        levelUpEvent += LevelUp;
    }

    public Animal(int animalId)
    {
        animalData = DataTableMgr.GetAnimalTable().Get(animalId);

        this.coinForSale = animalData.Sale_Coin;
        this.workload = animalData.Workload;
        this.stamina = (int)animalData.Stamina;
        this.coinForSale = animalData.Sale_Coin;
        this.costForLevelUp = animalData.Level_Up_Coin;
    }

    [SerializeField]
    private string costForLevelUp;
    public BigNumber CostForLevelUp
    {
        get
        {
            return new BigNumber(costForLevelUp);
        }
        set
        {
            costForLevelUp = value.ToString();
        }
    }


    [SerializeField]
    private string coinForSale;
    public BigNumber CoinForSale
    {
        get
        {
            return new BigNumber(coinForSale);
        }
        set
        {
            coinForSale = value.ToString();
        }
    }
    [SerializeField]
    private int stamina;
    public int Stamina { get => stamina; set => stamina = value; }
    [SerializeField]
    private int workload;
    public int Workload 
    { 
        get
        {
            return workload;
        }
        set
        {
            workload = value;
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
    public event Action levelUpEvent;

    public void LevelUp()
    {
        var animalClick = ClickableManager.CurrentClicked as AnimalClick;

        if (animalClick == null)
            return;

        var animals = FloorManager.GetFloor(animalWork.currentFloor).animals;

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
        BigNumber lvCoin = new BigNumber(animalData.Level_Up_Coin);
        if (CurrencyManager.currency[(int)CurrencyType.Coin] < new BigNumber(animalData.Level_Up_Coin)) // 임시 코드
            return;

        animalData = DataTableMgr.GetAnimalTable().Get(animalData.ID + 1);

        foreach(var a in animals)
        {
            if(a.animalWork.gameObject.GetInstanceID() == animalClick.gameObject.GetInstanceID())
            {
                a.animalData = animalData;
                
                break;
            }
        }
    }


    public void Sale()
    {

    }

    public void SetAnimal()
    {
        walkSpeed = 3f;
        runSpeed = 5f;
        idleTime = 2f;
    }

    public override string ToString()
    {
        return $"coin for sale : {CoinForSale}\nStamina : {Stamina}\nAutoHarvesting : {Workload}";
    }
}
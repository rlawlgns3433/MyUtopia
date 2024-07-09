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
        this.workload = ((BigInteger)animalData.Workload).ToString();
        this.stamina = (int)animalData.Stamina;
        this.coinForSale = animalData.Sale_Coin;
        this.costForLevelUp = animalData.Level_Up_Coin;
    }

    [SerializeField]
    private string costForLevelUp;
    public BigInteger CostForLevelUp
    {
        get
        {
            return costForLevelUp.ToBigInteger();
        }
        set
        {
            costForLevelUp = BigIntegerExtensions.ToString(value);
        }
    }


    [SerializeField]
    private string coinForSale;
    public BigInteger CoinForSale
    {
        get
        {
            return coinForSale.ToBigInteger();
        }
        set
        {
            coinForSale = BigIntegerExtensions.ToString(value);
        }
    }
    [SerializeField]
    private int stamina;
    public int Stamina { get => stamina; set => stamina = value; }
    [SerializeField]
    private string workload;
    public BigInteger Workload 
    { 
        get
        {
            return workload.ToBigInteger();
        }
        set
        {
            workload = BigIntegerExtensions.ToString(value);
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
        //Debug.Log(animalClick.gameObject.GetInstanceID());

        if (animalClick == null)
            return;

        if (animalData.Level == animalData.Level_Max)
            return;

        // 조건에 의해 레벨업
        if (CurrencyManager.currency[(int)CurrencyType.Coin] < CostForLevelUp)
            return;

        animalData = DataTableMgr.GetAnimalTable().Get(animalData.ID + 1);

        var animals = FloorManager.GetFloor(animalWork.currentFloor).animals;

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
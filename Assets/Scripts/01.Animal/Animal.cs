using System;
using System.Numerics;
using UnityEngine;

public class Animal : IGrowable, IMergable, ISaleable, IConductable, IMovable
{
    private AnimalData animalData;
    public Animal() { }
    public Animal(Animal other)
    {
        this.currentLevel = other.currentLevel;
        this.maxLevel = other.maxLevel;
        this.costForLevelUp = other.costForLevelUp;
        this.grade = other.grade;
        this.type = other.type;
        this.coinForSale = other.coinForSale;
        this.stamina = other.stamina;
        this.workload = other.workload;
        this.walkSpeed = other.walkSpeed;
        this.runSpeed = other.runSpeed;
        this.idleTime = other.idleTime;
        this.mergeId = other.mergeId;
    }

    public Animal(int animalId)
    {
        animalData = DataTableMgr.GetAnimalTable().Get(animalId);

        this.type = (AnimalType)animalData.Type;
        this.grade = animalData.Grade;
        this.currentLevel = animalData.Level;
        this.maxLevel = animalData.Level_Max; 
        this.coinForSale = animalData.Sale_Coin;
        this.workload = ((BigInteger)animalData.Workload).ToString();
        this.stamina = (int)animalData.Stamina;
        this.mergeId = animalData.Merge_ID;
        this.coinForSale = animalData.Sale_Coin;
        this.costForLevelUp = animalData.Level_Up_Coin;
    }

    [SerializeField]
    private int currentLevel;
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    [SerializeField]
    private int maxLevel;
    public int MaxLevel { get => maxLevel;}
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
    private int grade;
    public int Grade { get => grade; set => grade = value; }
    [SerializeField]
    private AnimalType type;
    public AnimalType Type { get => type; set => type = value; }

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

    [SerializeField]
    private int mergeId;
    public int MergeId { get => mergeId; set => mergeId = value; }

    public event Action clickEvent;
    public event Action levelUpEvent;

    public bool LevelUp()
    {
        // 조건에 의해 레벨업
        levelUpEvent?.Invoke();
        return false;
    }

    public Animal Merge(IMergable animal)
    {
        // 조건에 의해 머지
        return default;
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
        return $"current level : {CurrentLevel}\nmax level : {MaxLevel}\ncoin for sale : {CoinForSale}\nStamina : {Stamina}\nAutoHarvesting : {Workload}";
    }
}
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
        this.currentLevel = other.currentLevel;
        this.maxLevel = other.maxLevel;
        this.costForLevelUp = other.costForLevelUp;
        this.coinForSale = other.coinForSale;
        this.stamina = other.stamina;
        this.workload = other.workload;
        this.walkSpeed = other.walkSpeed;
        this.runSpeed = other.runSpeed;
        this.idleTime = other.idleTime;
    }

    public Animal(int animalId)
    {
        animalData = DataTableMgr.GetAnimalTable().Get(animalId);

        this.currentLevel = animalData.Level;
        this.maxLevel = animalData.Level_Max; 
        this.coinForSale = animalData.Sale_Coin;
        this.workload = animalData.Workload;
        this.stamina = (int)animalData.Stamina;
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
    private string workload;
    public BigNumber Workload 
    { 
        get
        {
            return new BigNumber(workload);
        }
        set
        {
            workload = value.ToString();
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

    public bool LevelUp()
    {
        // 조건에 의해 레벨업
        levelUpEvent?.Invoke();
        return false;
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
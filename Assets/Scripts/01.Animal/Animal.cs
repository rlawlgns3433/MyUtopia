using System;
using System.Numerics;
using TMPro;
using UnityEngine;

// Scriptable을 일반 클래스로 대체 예정
[CreateAssetMenu(fileName = "Animal", menuName = "Animal/AnimalName")]
public class Animal : AnimalStat, IGrowable, IMergable, ISaleable, IConductable
{
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
        this.clickEvent = other.clickEvent;
        this.levelUpEvent = other.levelUpEvent;
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
        return default;
    }

    public void Sale()
    {

    }

    public override string ToString()
    {
        return $"current level : {CurrentLevel}\nmax level : {MaxLevel}\ncoin for sale : {CoinForSale}\nStamina : {Stamina}\nAutoHarvesting : {Workload}";
    }
    //[SerializeField]
    //private bool isClicked = false;
    //public bool IsClicked
    //{
    //    get
    //    {
    //        clickEvent?.Invoke();
    //        return isClicked;
    //    }

    //    set
    //    {
    //        isClicked = value;
    //    }
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    clickEvent?.Invoke();
    //}
}
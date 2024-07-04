using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Animal", menuName = "Animal/Animal")]
public class Animal : AnimalStat, IGrowable, IMergable, ISaleable, IConductable
{
    [SerializeField]
    private int currentLevel;
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    [SerializeField]
    private int maxLevel;
    public int MaxLevel { get => maxLevel; set => maxLevel = value; }
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
            costForLevelUp = value.FormatBigInteger();
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
            coinForSale = value.FormatBigInteger();
        }
    }
    [SerializeField]
    private int stamina;
    public int Stamina { get => stamina; set => stamina = value; }
    [SerializeField]
    private string autoHarvesting;
    public BigInteger AutoHarvesting 
    { 
        get
        {
            return autoHarvesting.ToBigInteger();
        }
        set
        {
            autoHarvesting = value.FormatBigInteger();
        }
    }
    [SerializeField]
    private string autoProcessing;
    public BigInteger AutoProcessing
    {
        get
        {
            return autoProcessing.ToBigInteger();
        }
        set
        {
            autoProcessing = value.FormatBigInteger();
        }
    }
    [SerializeField]
    private string autoCreating;
    public BigInteger AutoCreating
    {
        get
        {
            return autoCreating.ToBigInteger();
        }
        set
        {
            autoCreating = value.FormatBigInteger();
        }
    }

    public event Action clickEvent;

    public bool LevelUp()
    {
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
        return $"current level : {CurrentLevel}\nmax level : {MaxLevel}\ncoin for sale : {CoinForSale}\nStamina : {Stamina}\nAutoHarvesting : {AutoHarvesting}";
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
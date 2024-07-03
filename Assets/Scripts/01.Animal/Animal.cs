using System;
using System.Numerics;

public abstract class Animal : IGrowable, IMergable, IClickable, ISaleable, IConductable
{
    public int CurrentLevel { get; set; }
    public int MaxLevel { get; set; }
    public BigInteger CostForLevelUp { get; set; }
    public int Grade { get; set; }

    public AnimalType Type { get; set; }
    private bool isFocused = false;
    public bool IsFocused
    {
        get
        {
            clickEvent?.Invoke();
            return isFocused;
        }

        set
        {
            isFocused = value;
        }
    }

    public BigInteger coinForSale { get; set; }
    public int Stamina { get; set; }
    public BigInteger AutoHarvesting { get; set; }
    public BigInteger AutoProcessing { get; set; }
    public BigInteger AutoCreating { get; set; }

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
}
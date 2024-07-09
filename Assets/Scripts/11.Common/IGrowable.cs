using System.Numerics;

public interface IGrowable
{
    public int CurrentLevel { get; set; }
    public int MaxLevel { get;}
    public event System.Action levelUpEvent;
    public BigNumber CostForLevelUp { get; set; }
    public bool LevelUp();
}
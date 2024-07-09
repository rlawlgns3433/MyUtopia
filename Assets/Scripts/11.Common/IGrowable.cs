using System.Numerics;

public interface IGrowable
{
    public event System.Action levelUpEvent;
    public BigNumber CostForLevelUp { get; set; }
    public void LevelUp();
}
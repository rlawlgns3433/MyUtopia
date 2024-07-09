using System.Numerics;

public interface IGrowable
{
    public event System.Action levelUpEvent;
    public BigInteger CostForLevelUp { get; set; }
    public void LevelUp();
}
using System.Numerics;

public interface IGrowable
{
    public int CurrentLevel { get; set; }
    public int MaxLevel { get; set; }
    public BigInteger CostForLevelUp { get; set; }
    public bool LevelUp();
}
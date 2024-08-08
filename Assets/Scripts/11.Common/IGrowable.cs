using System.Numerics;

public interface IGrowable
{
    public bool IsUpgrading { get; set; }
    public void LevelUp();
}
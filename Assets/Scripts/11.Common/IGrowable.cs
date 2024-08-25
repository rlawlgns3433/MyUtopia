using System.Numerics;

public interface IGrowable
{
    public double UpgradeTimeLeft { get; set; }
    public bool IsUpgrading { get; set; }
    public void LevelUp();
}
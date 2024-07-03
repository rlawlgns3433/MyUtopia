using System.Numerics;

public interface IConductable
{
    public int Stamina { get; set; }
    public BigInteger AutoHarvesting { get; set; }
    public BigInteger AutoProcessing { get; set; }
    public BigInteger AutoCreating { get; set; }
}
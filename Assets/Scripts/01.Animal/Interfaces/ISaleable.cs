using System.Numerics;

public interface ISaleable
{
    public BigInteger CoinForSale { get; set; }

    public void Sale();
}
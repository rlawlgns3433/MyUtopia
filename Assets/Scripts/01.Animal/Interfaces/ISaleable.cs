using System.Numerics;

public interface ISaleable
{
    public BigNumber CoinForSale { get; set; }

    public void Sale();
}
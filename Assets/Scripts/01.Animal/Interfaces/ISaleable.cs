using System.Numerics;

public interface ISaleable
{
    public BigInteger coinForSale { get; set; }

    public void Sale();
}
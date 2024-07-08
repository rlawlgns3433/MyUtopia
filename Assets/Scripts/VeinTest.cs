using System.Numerics;
using TMPro;

public class VeinTest : Observer
{
    public TextMeshProUGUI coin;
    public string format;
    public BigInteger big = new BigInteger();
    public override void Notify(Subject subject)
    {
        big += BigInteger.Parse("999");
        coin.text = string.Format(format, BigIntegerExtensions.ToString(big));
    }
}
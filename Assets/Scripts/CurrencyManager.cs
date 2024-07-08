public static class CurrencyManager
{
    private static readonly int currencyCount = 7;
    public static System.Numerics.BigInteger[] currency = new System.Numerics.BigInteger[currencyCount];

    public static void Init()
    {
        for (int i = 0; i < currencyCount; i++)
        {
            currency[i] = 0;
        }
    }
}

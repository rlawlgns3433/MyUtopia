public static class CurrencyManager
{
    private static readonly int currencyCount = 8;
    public static BigNumber[] currency = new BigNumber[currencyCount];

    public static void Init()
    {
        for (int i = 0; i < currencyCount; i++)
        {
            currency[i].Clear();
        }
    }
}

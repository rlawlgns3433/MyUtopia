using System.Collections.Generic;

public static class CurrencyManager
{
    public static CurrencyType[] currencyTypes =
    {
        CurrencyType.Diamond,
        CurrencyType.Coin,
        CurrencyType.CopperStone,
        CurrencyType.SilverStone,
        CurrencyType.GoldStone,
        CurrencyType.CopperIngot,
        CurrencyType.SilverIngot,
        CurrencyType.GoldIngot,
        CurrencyType.Craft
    };
    public static Dictionary<CurrencyType, BigNumber> currency = new Dictionary<CurrencyType, BigNumber>();

    public static void Init()
    {
        for(int i = 0; i < currencyTypes.Length; i++)
        {
            currency.Add(currencyTypes[i], BigNumber.Zero);
        }
    }
}

using System.Collections.Generic;

public static class CurrencyManager
{
    public static CurrencyType[] currencyTypes =
    {
        CurrencyType.Diamond,
        CurrencyType.Coin,
    };

    public static CurrencyProductType[] productTypes =
    {
        CurrencyProductType.CopperStone,
        CurrencyProductType.SilverStone,
        CurrencyProductType.GoldStone,
        CurrencyProductType.CopperIngot,
        CurrencyProductType.SilverIngot,
        CurrencyProductType.GoldIngot,
    };

    public static Dictionary<CurrencyType, BigNumber> currency = new Dictionary<CurrencyType, BigNumber>();
    public static Dictionary<CurrencyProductType, BigNumber> product = new Dictionary<CurrencyProductType, BigNumber>();

    public static void Init()
    {
        for(int i = 0; i < currencyTypes.Length; ++i)
        {
            if (currency.ContainsKey(currencyTypes[i]))
                continue;
            currency.Add(currencyTypes[i], BigNumber.Zero);
        }

        for(int i = 0; i < productTypes.Length; ++i)
        {
            if (product.ContainsKey(productTypes[i]))
                continue;
            product.Add(productTypes[i], BigNumber.Zero);
        }

        currency[CurrencyType.Coin] = new BigNumber(1000);
    }
}

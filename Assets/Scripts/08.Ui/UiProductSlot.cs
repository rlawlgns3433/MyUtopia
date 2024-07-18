using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiProductSlot : MonoBehaviour
{
    public Image imagePortrait;
    public TextMeshProUGUI textProductName;
    public ItemStat itemStat;
    public Button buttonSale;

    public async void SetData(ItemStat itemStat)
    {
        if (itemStat == null)
            return;

        this.itemStat = itemStat;
        imagePortrait.sprite = await itemStat.ItemData.GetImage();
        textProductName.text = itemStat.ItemData.GetName();
        buttonSale.onClick.AddListener(OnClickSale);
    }

    public void ClearData()
    {
        this.itemStat = null;
        imagePortrait.sprite = null;
        textProductName.text = string.Empty;
    }

    public void OnClickSale()
    {
        /*
         1. 아이템의 가격을 가져온다.
         2. 아이템을 판매한다.
         3. 아이템의 가격만큼 currency를 올린다.
         4. 아이템의 리스트에서 개수를 하나 줄인다.
         5. 아이템을 삭제한다. ClearData
         */
        CurrencyType type = (CurrencyType)itemStat.Sell_Resource_ID;
        BigNumber price = itemStat.Sell_Price.ToBigNumber();

        CurrencyManager.currency[type] += price;

        CurrencyManager.currency[CurrencyType.Craft] -= 1;

        ClearData();

        UiManager.Instance.productsUi.Refresh();
    }
}

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
         1. �������� ������ �����´�.
         2. �������� �Ǹ��Ѵ�.
         3. �������� ���ݸ�ŭ currency�� �ø���.
         4. �������� ����Ʈ���� ������ �ϳ� ���δ�.
         5. �������� �����Ѵ�. ClearData
         */
        CurrencyType type = (CurrencyType)itemStat.Sell_Resource_ID;
        BigNumber price = itemStat.Sell_Price.ToBigNumber();

        CurrencyManager.currency[type] += price;

        CurrencyManager.currency[CurrencyType.Craft] -= 1;

        ClearData();

        UiManager.Instance.productsUi.Refresh();
    }
}

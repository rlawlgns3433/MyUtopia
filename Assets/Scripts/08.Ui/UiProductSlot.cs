using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Purchasing;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UiProductSlot : MonoBehaviour
{
    public Image imagePortrait;
    public TextMeshProUGUI textProductName;
    public ItemStat itemStat;
    public Button buttonSale;

    public void Awake()
    {
        buttonSale.onClick.AddListener(OnClickSale);
    }

    public async void SetData(ItemStat itemStat)
    {
        if (itemStat == null)
            return;

        this.itemStat = itemStat;
        imagePortrait.sprite = await itemStat.ItemData.GetImage();
        textProductName.text = itemStat.ItemData.GetName();
    }

    public void ClearData()
    {
        this.itemStat = null;

        imagePortrait.sprite = Addressables.LoadAssetAsync<Sprite>("Transparency").WaitForCompletion();
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
        if (itemStat == null)
            return;

        SoundManager.Instance.OnClickButton(SoundType.Selling);
        CurrencyType type = (CurrencyType)itemStat.Sell_Resource_ID;
        BigNumber price = itemStat.Sell_Price.ToBigNumber();

        CurrencyManager.currency[type] += price;


        var storage = FloorManager.Instance.floors["B3"].storage as StorageProduct;

        storage.DecreaseProduct(itemStat.Item_ID);

        ClearData();

        var uiProduct = UiManager.Instance.productsUi;

        for (int i = 0; i < uiProduct.uiProducts.Count; i++)
        {
            Destroy(uiProduct. uiProducts[i].gameObject);
        }
        uiProduct.uiProducts.Clear();

        uiProduct.Refresh();
        if (FloorManager.Instance.touchManager.tutorial != null)
        {
            if (FloorManager.Instance.touchManager.tutorial.progress == TutorialProgress.SellItem)
            {
                FloorManager.Instance.touchManager.tutorial.SetTutorialProgress();
            }
        }
        MissionManager.Instance.AddMissionCountSellItem();
    }
}

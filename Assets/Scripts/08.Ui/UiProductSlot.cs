using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiProductSlot : MonoBehaviour
{
    public Image imagePortrait;
    public TextMeshProUGUI textProductName;
    public ItemStat itemStat;

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
        imagePortrait.sprite = null;
        textProductName.text = string.Empty;
    }
}

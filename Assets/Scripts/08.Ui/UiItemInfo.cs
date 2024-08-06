using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemInfo : MonoBehaviour
{
    private static readonly string formatCount = "{0}/{1}";
    public Image imageProfile;
    public Image imageComplete;
    public TextMeshProUGUI textCount;

    public ItemStat itemStat;
    public ResourceStat resourceStat;
    private BigNumber count = BigNumber.Zero;
    public BigNumber requireCount = BigNumber.Zero;
    public int requireCountInt
    {
        get
        {
            return requireCount.ToInt();
        }
    }

    public bool IsCompleted
    {
        get
        {
            return count >= requireCount;
        }
    }

    public void ClearData()
    {
        imageProfile.sprite = null;
        textCount.text = string.Empty;
        itemStat = null;
        resourceStat = null;
        count = BigNumber.Zero;
        requireCount = BigNumber.Zero;
    }

    public async void SetData(ItemStat itemStat, BigNumber count, string requireCount)
    {
        if (itemStat == null)
            return;
        this.itemStat = itemStat;
        this.count = count;
        this.requireCount = new BigNumber(requireCount);

        imageProfile.sprite = await this.itemStat.ItemData.GetImage();
        imageProfile.type = Image.Type.Simple;
        imageProfile.preserveAspect = true;

        textCount.text = string.Format(formatCount, this.count, this.requireCount); // 스토리지 프로덕트에 있는 아이템 개수 가져오기
        imageComplete.gameObject.SetActive(IsCompleted);
    }

    public async void SetData(ResourceStat resourceStat, BigNumber count, string requireCount)
    {
        if (resourceStat == null)
            return;
        this.resourceStat = resourceStat;
        this.count = count;
        this.requireCount = new BigNumber(requireCount);

        imageProfile.sprite = await this.resourceStat.ResourceData.GetImage();
        imageProfile.type = Image.Type.Simple;
        imageProfile.preserveAspect = true;

        textCount.text = string.Format(formatCount, this.count, this.requireCount);
        imageComplete.gameObject.SetActive(IsCompleted);
    }
}

using UnityEngine;
using DG.Tweening;

public class DTUiPanel : MonoBehaviour
{
    public UiPanels panel;

    public float startUpDuration = 1.0f;
    public float finishDuration = 1.0f;
    public bool isFinishing = false;
    private bool isActive = false;
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            if(value)
            {
                if (isFinishing)
                    return;

                isActive = value;
                gameObject.SetActive(isActive);
                transform.DOScale(Vector3.one, startUpDuration).SetEase(startUpEase); // 스케일 조절중
            }
            else
            {
                if(isFinishing)
                    UiManager.Instance.panelBlock.SetActive(true);

                transform.DOScale(Vector3.zero, finishDuration).SetEase(finishEase).OnComplete(
                () => 
                { 
                    isActive = value;
                    gameObject.SetActive(isActive);
                    isFinishing = false;
                    UiManager.Instance.panelBlock.SetActive(false);
                });
            }
        }
    }


    public Ease startUpEase = Ease.InOutQuad;
    public Ease finishEase = Ease.InOutQuad;

    private void Awake()
    {
        DOTween.Init();
        transform.localScale = Vector3.zero;
    }
}

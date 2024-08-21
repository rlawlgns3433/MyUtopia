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
                if(FloorManager.Instance.touchManager.tutorial != null)
                    FloorManager.Instance.touchManager.tutorial.activingUiPanel = true;
                if (isFinishing)
                    return;

                isActive = value;
                gameObject.SetActive(isActive);
                UiManager.Instance.panelBlock.SetActive(true);

                transform.DOScale(Vector3.one, startUpDuration).SetEase(startUpEase).OnComplete(
                    () =>
                    {
                        UiManager.Instance.panelBlock.SetActive(false);
                        if (FloorManager.Instance.touchManager.tutorial != null)
                            FloorManager.Instance.touchManager.tutorial.activingUiPanel = false;
                    });
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
                    if (FloorManager.Instance.touchManager.tutorial != null)
                        FloorManager.Instance.touchManager.tutorial.activingUiPanel = false;
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

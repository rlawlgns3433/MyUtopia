using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : Singleton<LoadingManager>, ISingletonCreatable
{
    public GameObject loadingPanel;
    public GameObject loadingImage;
    public float worldBgmValue = 1;
    public float worldSfxValue = 1;
    public bool worldBgmIsMute = false;
    public bool worldSfxIsMute = false;
    public bool isLoading = false;
    private async void Awake()
    {
        await UniTask.WaitUntil(() => this != null);
        if (loadingPanel == null)
        {
            loadingPanel = transform.Find("LoadingPanel").gameObject;
        }
        if(loadingImage == null)
        {
            await UniTask.WaitUntil(() => loadingPanel != null);
            loadingImage = loadingPanel.transform.Find("Loading").gameObject;
        }
        //Application.quitting += SaveSoundValue;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveSoundValue();
        }
    }

    public bool ShouldBeCreatedInScene(string sceneName)
    {
        return false;
    }

    public void ShowLoadingPanel()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
            isLoading = true;
            LoadingImageRotate();
        }
    }

    public void HideLoadingPanel()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
            isLoading = false;
        }
    }

    public async UniTask FadeIn(float duration)
    {
        var canvasGroup = loadingPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = loadingPanel.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0;
        await canvasGroup.DOFade(1, duration).AsyncWaitForCompletion();
    }

    public async UniTask FadeOut(float duration)
    {
        var canvasGroup = loadingPanel.GetComponent<CanvasGroup>();
        await canvasGroup.DOFade(0, duration).AsyncWaitForCompletion();
        loadingPanel.SetActive(false);
    }

    private void LoadingImageRotate()
    {
        var image = loadingImage.GetComponent<Image>();
        image.rectTransform.DORotate(new Vector3(0, 0, -360), 2f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void SaveSoundValue()
    {
        PlayerPrefs.SetFloat("BgmValue", Instance.worldBgmValue);
        PlayerPrefs.SetFloat("SfxValue", Instance.worldSfxValue);
        PlayerPrefs.SetInt("IsBgmMute", Instance.worldBgmIsMute ? 1 : 0);
        PlayerPrefs.SetInt("IsSfxMute", Instance.worldSfxIsMute ? 1 : 0);
    }

    private void SetValue()
    {
        if (PlayerPrefs.HasKey("BgmValue"))
        {
            Instance.worldBgmValue = PlayerPrefs.GetFloat("BgmValue");
        }
        else
        {
            Instance.worldBgmValue = 1;
        }
        if (PlayerPrefs.HasKey("SfxValue"))
        {
            Instance.worldSfxValue = PlayerPrefs.GetFloat("SfxValue");
        }
        else
        {
            Instance.worldSfxValue = 1;
        }
        if (PlayerPrefs.HasKey("IsBgmMute"))
        {
            if (PlayerPrefs.GetInt("IsBgmMute") == 1)
            {
                Instance.worldBgmIsMute = true;
            }
            else if (PlayerPrefs.GetInt("IsBgmMute") == 0)
            {
                Instance.worldBgmIsMute = false;
            }
        }
        else
        {
            Instance.worldBgmIsMute = false;
        }
        if (PlayerPrefs.HasKey("IsSfxMute"))
        {
            if (PlayerPrefs.GetInt("IsSfxMute") == 1)
            {
                Instance.worldSfxIsMute = true;
            }
            else if (PlayerPrefs.GetInt("IsSfxMute") == 0)
            {
                Instance.worldSfxIsMute = false;
            }
        }
        else
        {
            Instance.worldSfxIsMute = false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSetting : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public Image imageBgmMute;
    public Image imageSfxMute;

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener((float value) =>
        {
            SoundManager.Instance.IsBgmMute = false;
            imageBgmMute.gameObject.SetActive(false);
            SoundManager.Instance.bgmAudioSource.volume = value;
        });

        sfxSlider.onValueChanged.AddListener((float value) =>
        {
            SoundManager.Instance.IsSfxMute = false;
            imageSfxMute.gameObject.SetActive(false);
            SoundManager.Instance.sfxAudioSource.volume = value;
        });
    }

    public void OnClickMuteBgm()
    {
        if (SoundManager.Instance.IsBgmMute)
        {
            SoundManager.Instance.IsBgmMute = false;
            imageBgmMute.gameObject.SetActive(false);
            SoundManager.Instance.bgmAudioSource.mute = SoundManager.Instance.IsBgmMute;
            return;
        }
        SoundManager.Instance.bgmAudioSource.mute = true;
        imageBgmMute.gameObject.SetActive(true);
        SoundManager.Instance.IsBgmMute = true;
    }

    public void OnClickMuteSfx()
    {
        if (SoundManager.Instance.IsSfxMute)
        {
            SoundManager.Instance.IsSfxMute = false;
            imageSfxMute.gameObject.SetActive(false);
            SoundManager.Instance.sfxAudioSource.mute = SoundManager.Instance.IsSfxMute;
            return;
        }
        SoundManager.Instance.sfxAudioSource.mute = true;
        imageSfxMute.gameObject.SetActive(true);
        SoundManager.Instance.IsSfxMute = true;
    }

    public void SetSlider()
    {
        bgmSlider.value = SoundManager.Instance.bgmAudioSource.volume;
        sfxSlider.value = SoundManager.Instance.sfxAudioSource.volume;
        SetMute();
    }

    public void SetMute()
    {
        if (SoundManager.Instance.IsBgmMute)
        {
            imageBgmMute.gameObject.SetActive(true);
            SoundManager.Instance.IsBgmMute = true;
        }
        else
        {
            imageBgmMute.gameObject.SetActive(false);
            SoundManager.Instance.IsBgmMute = false;
        }
        if (SoundManager.Instance.IsSfxMute)
        {
            imageSfxMute.gameObject.SetActive(true);
            SoundManager.Instance.IsSfxMute = true;
        }
        else
        {
            imageSfxMute.gameObject.SetActive(false);
            SoundManager.Instance.IsSfxMute = false;
        }
    }
}

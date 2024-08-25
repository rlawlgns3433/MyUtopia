using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapUiSetting : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public Image imageBgmMute;
    public Image imageSfxMute;

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener((float value) =>
        {
            WorldMapSoundManager.Instance.IsBgmMute = false;
            imageBgmMute.gameObject.SetActive(false);
            WorldMapSoundManager.Instance.bgmAudioSource.volume = value;
        });

        sfxSlider.onValueChanged.AddListener((float value) =>
        {
            WorldMapSoundManager.Instance.IsSfxMute = false;
            imageSfxMute.gameObject.SetActive(false);
            WorldMapSoundManager.Instance.sfxAudioSource.volume = value;
        });
    }

    public void OnClickMuteBgm()
    {
        if (WorldMapSoundManager.Instance.IsBgmMute)
        {
            WorldMapSoundManager.Instance.IsBgmMute = false;
            imageBgmMute.gameObject.SetActive(false);
            WorldMapSoundManager.Instance.bgmAudioSource.mute = WorldMapSoundManager.Instance.IsBgmMute;
            return;
        }
        WorldMapSoundManager.Instance.bgmAudioSource.mute = true;
        imageBgmMute.gameObject.SetActive(true);
        WorldMapSoundManager.Instance.IsBgmMute = true;
    }

    public void OnClickMuteSfx()
    {
        if (WorldMapSoundManager.Instance.IsSfxMute)
        {
            WorldMapSoundManager.Instance.IsSfxMute = false;
            imageSfxMute.gameObject.SetActive(false);
            WorldMapSoundManager.Instance.sfxAudioSource.mute = WorldMapSoundManager.Instance.IsSfxMute;
            return;
        }
        WorldMapSoundManager.Instance.sfxAudioSource.mute = true;
        imageSfxMute.gameObject.SetActive(true);
        WorldMapSoundManager.Instance.IsSfxMute = true;
    }

    public void SetSlider()
    {
        bgmSlider.value = WorldMapSoundManager.Instance.bgmAudioSource.volume;
        sfxSlider.value = WorldMapSoundManager.Instance.sfxAudioSource.volume;
        SetMute();
    }

    public void SetMute()
    {
        if(WorldMapSoundManager.Instance.IsBgmMute)
        {
            imageBgmMute.gameObject.SetActive(true);
            WorldMapSoundManager.Instance.IsBgmMute = true;
        }
        else
        {
            imageBgmMute.gameObject.SetActive(false);
            WorldMapSoundManager.Instance.IsBgmMute = false;
        }
        if(WorldMapSoundManager.Instance.IsSfxMute)
        {
            imageSfxMute.gameObject.SetActive(true);
            WorldMapSoundManager.Instance.IsSfxMute = true;
        }
        else
        {
            imageSfxMute.gameObject.SetActive(false);
            WorldMapSoundManager.Instance.IsSfxMute = false;
        }
    }
}

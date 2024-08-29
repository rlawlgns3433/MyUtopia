using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapSoundManager : Singleton<WorldMapSoundManager>, ISingletonCreatable
{
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    [SerializeField]
    private AudioClip[] bgmClips;
    public List<AudioClip> sfxClips = new List<AudioClip>();
    public SoundType soundType;
    public WorldMapUiSetting setting;

    private bool isBgmMute = false;
    public bool IsBgmMute
    {
        get => isBgmMute;
        set
        {
            isBgmMute = value;
            bgmAudioSource.mute = isBgmMute;
        }
    }
    private bool isSfxMute;
    public bool IsSfxMute
    {
        get => isSfxMute;
        set
        {
            isSfxMute = value;
            sfxAudioSource.mute = isSfxMute;
        }
    }

    private async void Start()
    {
        await SetVolume();
        Play();
    }

    private void Play()
    {
        bgmAudioSource.clip = bgmClips[0];
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }
    public void OnClickButton(SoundType type)
    {
        if (sfxAudioSource.isPlaying)
        {
            switch (soundType)
            {
                case SoundType.Selling:
                case SoundType.Caution:
                case SoundType.PopUpClose:  // 플레이 하지 않음
                case SoundType.PopUpOpen:  // 플레이 하지 않음
                case SoundType.GetAnimal: // 플레이 하지 않음
                    return;
                default:
                    break;
            }
        }

        soundType = type;
        sfxAudioSource.clip = sfxClips[(int)soundType];
        sfxAudioSource.loop = false;
        sfxAudioSource.Play();
    }

    public bool ShouldBeCreatedInScene(string sceneName)
    {
        return sceneName == "WorldMap";
    }

    public void SaveVolume()
    {
        LoadingManager.Instance.worldBgmValue = Instance.bgmAudioSource.volume;
        LoadingManager.Instance.worldSfxValue = Instance.sfxAudioSource.volume;
        LoadingManager.Instance.worldBgmIsMute = Instance.IsBgmMute;
        LoadingManager.Instance.worldSfxIsMute = Instance.IsSfxMute;
        LoadingManager.Instance.SaveSoundValue();
    }

    public async UniTask SetVolume()
    {
        await UniTask.WaitUntil(() => LoadingManager.Instance != null);
        Set();
    }

    private void Set()
    {
        if (PlayerPrefs.HasKey("BgmValue"))
        {
            Instance.bgmAudioSource.volume = PlayerPrefs.GetFloat("BgmValue");
        }
        else
        {
            Instance.bgmAudioSource.volume = 1;
            PlayerPrefs.SetFloat("BgmValue", 1);
        }
        if (PlayerPrefs.HasKey("SfxValue"))
        {
            Instance.sfxAudioSource.volume = PlayerPrefs.GetFloat("SfxValue");
        }
        else
        {
            Instance.sfxAudioSource.volume = 1;
            PlayerPrefs.SetFloat("SfxValue", 1);
        }
        if (PlayerPrefs.HasKey("IsBgmMute"))
        {
            if (PlayerPrefs.GetInt("IsBgmMute") == 1)
            {
                Instance.IsBgmMute = true;
            }
            else if (PlayerPrefs.GetInt("IsBgmMute") == 0)
            {
                Instance.IsBgmMute = false;
            }
        }
        else
        {
            Instance.IsBgmMute = false;
            PlayerPrefs.SetInt("IsBgmMute", 0);
        }
        if (PlayerPrefs.HasKey("IsSfxMute"))
        {
            if (PlayerPrefs.GetInt("IsSfxMute") == 1)
            {
                Instance.IsSfxMute = true;
            }
            else if (PlayerPrefs.GetInt("IsSfxMute") == 0)
            {
                Instance.IsSfxMute = false;
            }
        }
        else
        {
            Instance.IsSfxMute = false;
            PlayerPrefs.SetInt("IsSfxMute", 0);
        }
        //Instance.bgmAudioSource.volume = LoadingManager.Instance.worldBgmValue;
        //Instance.sfxAudioSource.volume = LoadingManager.Instance.worldSfxValue;
        //Instance.IsBgmMute = LoadingManager.Instance.worldBgmIsMute;
        //Instance.IsSfxMute = LoadingManager.Instance.worldSfxIsMute;
        //Debug.Log($"WorldSetB{bgmAudioSource.volume}");

    }
}

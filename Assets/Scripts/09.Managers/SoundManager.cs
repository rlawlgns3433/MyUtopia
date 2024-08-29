using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>, ISingletonCreatable
{
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    [SerializeField]
    private AudioClip[] bgmClips;
    public List<AudioClip> sfxClips = new List<AudioClip>();
    public SoundType soundType;

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
                default :
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
        return sceneName == "SampleScene CBTJH";
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

    public void Set()
    {
        Instance.bgmAudioSource.volume = LoadingManager.Instance.worldBgmValue;
        Instance.sfxAudioSource.volume = LoadingManager.Instance.worldSfxValue;
        Instance.IsBgmMute = LoadingManager.Instance.worldBgmIsMute;
        Instance.IsSfxMute = LoadingManager.Instance.worldSfxIsMute;
    }
}
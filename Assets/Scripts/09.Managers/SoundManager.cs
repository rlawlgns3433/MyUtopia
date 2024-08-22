using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
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

    private void Start()
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
                //case SoundType.MergeAnimal:
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


}
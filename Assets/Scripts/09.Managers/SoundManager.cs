using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource sfxAudioSource;
    [SerializeField]
    private AudioClip[] bgmClips;
    public List<AudioClip> sfxClips = new List<AudioClip>();

    private void Start()
    {
        audioSource.clip = bgmClips[0];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void OnClickButton()
    {
        sfxAudioSource.clip = sfxClips[0];
        sfxAudioSource.loop = false;
        sfxAudioSource.Play();
    }
}
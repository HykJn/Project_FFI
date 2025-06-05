using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields
    public static SoundManager Instance => instance;

    public AudioSource BGMChannel => bgmChannel;
    public AudioSource SFXChannel => sfxChannel;

    public float MainVolume
    {
        get => mainVolume;
        set
        {
            mainVolume = Mathf.Clamp01(value / 100f);
            bgmChannel.volume = mainVolume * bgmVolume;
            sfxChannel.volume = mainVolume * sfxVolume;
        }
    }

    public float BGMVolume
    {
        get => bgmVolume;
        set
        {
            bgmVolume = Mathf.Clamp01(value / 100f);
            bgmChannel.volume = mainVolume * bgmVolume;
        }
    }

    public float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = Mathf.Clamp01(value / 100f);
            sfxChannel.volume = mainVolume * sfxVolume;
        }
    }

    public bool BGMAutoPlay { get; set; } = false;

    private static SoundManager instance = null;

    [Header("Channels")]
    [SerializeField] AudioSource bgmChannel;
    [SerializeField] AudioSource sfxChannel;
    [Header("Volume")]
    [SerializeField, Range(0, 1)] float mainVolume = 1f;
    [SerializeField, Range(0, 1)] float bgmVolume = 1f;
    [SerializeField, Range(0, 1)] float sfxVolume = 1f;
    [Header("Audio Clips")]
    [SerializeField] Dict<BGMID, AudioClip> bgmClips = new();
    [SerializeField] Dict<SFXID, AudioClip> sfxClips = new();
    private Queue<AudioClip> bgmQueue = new();
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        foreach(KVPair<BGMID, AudioClip> pair in bgmClips)
        {
            bgmQueue.Enqueue(pair.Value);
        }

        bgmChannel.volume = mainVolume * bgmVolume;
        sfxChannel.volume = mainVolume * sfxVolume;
    }

    private void Update()
    {
        BGM_AutoPlay();
    }
    #endregion

    #region Methods
    //BGMs
    public void BGM_Play(AudioClip clip)
    {
        bgmChannel.clip = clip;
        bgmChannel.Play();
    }
    public void BGM_Play(BGMID id) => BGM_Play(bgmClips[id]);
    public void BGM_Play() => bgmChannel.Play();

    public void BGM_PlayWithCheck(AudioClip clip)
    {
        if (!bgmChannel.isPlaying) BGM_Play(clip);
    }

    public void BGM_PlayWithCheck(BGMID id) => BGM_PlayWithCheck(bgmClips[id]);

    public void BGM_PlayWithCheck()
    {
        if (!bgmChannel.isPlaying) BGM_Play();
    }

    public void BGM_AutoPlay()
    {
        if (!BGMAutoPlay) return;
        
        if (!BGMChannel.isPlaying)
        {
            AudioClip clip = bgmQueue.Dequeue();
            BGM_Play(clip);
            bgmQueue.Enqueue(clip);
        }
    }

    public void BGM_Stop()
    {
        bgmChannel.Stop();
    }

    //SFXs
    public void SFX_Play(AudioClip clip)
    {
        sfxChannel.clip = clip;
        sfxChannel.Play();
    }
    public void SFX_Play(SFXID id) => SFX_Play(sfxClips[id]);
    public void SFX_Play() => sfxChannel.Play();

    public void SFX_PlayWithCheck(AudioClip clip)
    {
        if (!sfxChannel.isPlaying) SFX_Play(clip);
    }
    public void SFX_PlayWithCheck(SFXID id) => SFX_PlayWithCheck(sfxClips[id]);

    public void SFX_PlayWithCheck()
    {
        if (!sfxChannel.isPlaying) SFX_Play();
    }

    public void SFX_PlayOneShot(AudioClip clip)
    {
        SFXChannel.PlayOneShot(clip);
    }
    public void SFX_PlayOneShot(SFXID id) => SFX_PlayOneShot(sfxClips[id]);
    public void SFX_PlayOneShot() => SFXChannel.PlayOneShot(sfxChannel.clip);

    public void SFX_PlayOneShotWithCheck(AudioClip clip)
    {
        if (!sfxChannel.isPlaying) SFX_PlayOneShot(clip);
    }
    public void SFX_PlayOneShotWithCheck(SFXID id) => SFX_PlayOneShotWithCheck(sfxClips[id]);

    public void SFX_PlayOneShotWithCheck()
    {
        if (!sfxChannel.isPlaying) SFX_PlayOneShot();
    }

    public void SFX_Stop()
    {
        sfxChannel.Stop();
    }

    #endregion
}

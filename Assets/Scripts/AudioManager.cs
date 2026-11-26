using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;

public class AudioManager : MonoBehaviour
{
    private SaveManager saveManager;
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    [SerializeField] private AudioMixerGroup seMixerGroup;

    [Header("Audio Source")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [Header("Sound Lists")]
    [SerializeField] private List<AudioClip> bgmList = new List<AudioClip>();
    [SerializeField] private List<AudioClip> seList = new List<AudioClip>();
    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> seDictionary = new Dictionary<string, AudioClip>();

    private bool _isMuted;
    public bool IsMuted => _isMuted;

    [Inject]
    public void Construct(SaveManager saveManager)
    {
        this.saveManager = saveManager;
        _isMuted = saveManager.Data.sound.isMuted;
        ApplyMute(_isMuted);
    }

    private void Awake()
    {
        if (bgmSource) bgmSource.outputAudioMixerGroup = bgmMixerGroup;
        if (seSource) seSource.outputAudioMixerGroup = seMixerGroup;

        foreach (var bgm in bgmList)
        {
            bgmDictionary.Add(bgm.name, bgm);
        }
        foreach (var se in seList)
        {
            seDictionary.Add(se.name, se);
        }
        PlayBGM("bgm_stage");
    }

    public void PlayBGM(string bgmName)
    {
        if (bgmDictionary.TryGetValue(bgmName, out AudioClip bgm))
        {
            bgmSource.clip = bgm;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySE(string seName)
    {
        if (seDictionary.TryGetValue(seName, out AudioClip se))
        {
            seSource.PlayOneShot(se);
        }
    }

    public void SetMuted(bool isMuted)
    {
        ApplyMute(isMuted);
        saveManager.Data.sound.isMuted = isMuted;
        saveManager.Save();
    }

    public void ApplyMute(bool isMuted)
    {
        _isMuted = isMuted;
        bgmSource.mute = _isMuted;
        seSource.mute = _isMuted;
    }
}

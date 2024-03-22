using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]private AudioMixer masterMixer;
    [SerializeField]private Slider musicSlider;
    [SerializeField]private Slider sfxSlider;
    
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";


    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
        SetSFXVolume(PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f));
    }
    private void OnDisable()
    {
       SaveVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }

    private void SetMusicVolume(float value)
    {
        masterMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    private void SetSFXVolume(float value)
    {
        masterMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}

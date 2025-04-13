using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer SFXMixer;
    public AudioMixer MusicMixer;
    public CanvasGroup PressAnyKeyText;
    private bool _isLoading;
    private bool _settingsMenuOpen;
    public GameObject settingsMenu;
    public Toggle fullscreenToggle;
    public Slider SFXVolumeSlider;
    public Slider MusicVolumeSlider;
    
    private void Start()
    {
        //if we dont have save data set default volume values
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 0.25f);
            PlayerPrefs.SetFloat("MusicVolume", 0.25f);
            PlayerPrefs.SetInt("Fullscreen", 1);
        }

        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1 ? true : false;
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;

        FadeManager.Instance.FadeIn();
        Application.targetFrameRate = -1;
        LevelSystem.Instance.level = 1;
        PressAnyKeyText.DOFade(0f, 0.7f).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        Debug.Log(EventSystem.current.IsPointerOverGameObject());
        if (_isLoading) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.Save();
            Application.Quit();
        }
        else if (Input.anyKey && !EventSystem.current.IsPointerOverGameObject() && !_settingsMenuOpen)
        {
            PlayerPrefs.Save();
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        _isLoading = true;
        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Scene 1");
    }

    public void SettingsMenuButton()
    {
        _settingsMenuOpen = !_settingsMenuOpen;
        settingsMenu.SetActive(_settingsMenuOpen);
    }

    public void SFXSlider()
    {
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolumeSlider.value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
    }

    public void MusicSlider()
    {
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolumeSlider.value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
    }

    public void FullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
    }
}

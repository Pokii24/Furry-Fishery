using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public bool isPaused; 
    public CanvasGroup pauseMenu;
    public RectTransform resumeButton;
    public RectTransform volumeButton;
    public RectTransform mainMenuButton;
    public GameObject volumeSettings;
    public Slider SFXVolume;
    public Slider MusicVolume;
    public AudioMixer SFXMixer;
    public AudioMixer MusicMixer;
    private bool _isPausing;

    public Transform defaultRodAnim;
    public Transform looseLineAnim;
    public Transform bobberAnim;

    public AudioSource pauseMenuMusic; 
    public AudioSource backgroundMusic;

    //making sure only one script is running and killing itself if so
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SFXVolume.value = PlayerPrefs.GetFloat("SFXVolume");
        MusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void PauseGame()
    {
        if (_isPausing || volumeSettings.activeSelf) return;
        isPaused = !isPaused;
        if (isPaused)
        {
            StartCoroutine(PauseCoroutine());
            StartCoroutine(PauseMusic());
        }
        else
        {
            StartCoroutine(UnPauseCoroutine());
            StartCoroutine(BackgroundMusic());
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scene 0");
    }

    IEnumerator PauseCoroutine()
    {
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        _isPausing = true;
        resumeButton.anchoredPosition = new Vector2(0, 318);
        volumeButton.anchoredPosition = new Vector2(0, 318);
        mainMenuButton.anchoredPosition = new Vector2(0, 318);
        resumeButton.DOAnchorPosY(0, 0.75f);
        volumeButton.DOAnchorPosY(-318, 0.75f);
        mainMenuButton.DOAnchorPosY(-636, 0.75f);
        defaultRodAnim.DOLocalMoveX(11, 1f);
        looseLineAnim.DOLocalMoveX(11, 1f);
        bobberAnim.DOLocalMoveX(11, 1f);
        yield return new WaitForSeconds(0.75f);
        pauseMenu.interactable = true;
        pauseMenu.blocksRaycasts = true;
        _isPausing = false;
    }

    IEnumerator UnPauseCoroutine()
    {
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        _isPausing = true;
        resumeButton.DOAnchorPosY(318, 0.75f);
        volumeButton.DOAnchorPosY(318, 0.75f);
        mainMenuButton.DOAnchorPosY(318, 0.75f);
        defaultRodAnim.DOLocalMoveX(-0.03568355f, 1f);
        looseLineAnim.DOLocalMoveX(-0.03568355f, 1f);
        bobberAnim.DOLocalMoveX(-0.03568355f, 1f);
        yield return new WaitForSeconds(0.75f);
        _isPausing = false;
    }

    IEnumerator PauseMusic()
    {
        backgroundMusic.DOFade(0, 0.75f);
        yield return new WaitForSeconds(0.75f);
        backgroundMusic.Stop();
        pauseMenuMusic.Play();
        pauseMenuMusic.DOFade(1, 0.75f);
    }

    IEnumerator BackgroundMusic()
    {
        pauseMenuMusic.DOFade(0, 0.75f);
        yield return new WaitForSeconds(0.75f);
        pauseMenuMusic.Stop();
        backgroundMusic.Play();
        backgroundMusic.DOFade(1, 0.75f);
    }

    public void VolumeSettings()
    {
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        volumeSettings.SetActive(true);
    }

    public void VolumeSettingsBack()
    {
        volumeSettings.SetActive(false);
        pauseMenu.interactable = true;
        pauseMenu.blocksRaycasts = true;
        PlayerPrefs.Save();
    }

    public void SFXSlider()
    {
        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolume.value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume.value);
    }

    public void MusicSlider()
    {
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolume.value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume.value);
    }
}

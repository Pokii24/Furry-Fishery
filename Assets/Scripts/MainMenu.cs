using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixer SFXMixer;
    public AudioMixer MusicMixer;
    public CanvasGroup PressAnyKeyText;
    private bool _isLoading;
    
    private void Start()
    {
        //if we dont have save data set default volume values
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 0.25f);
            PlayerPrefs.SetFloat("MusicVolume", 0.25f);
        }

        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);

        FadeManager.Instance.FadeIn();
        Application.targetFrameRate = -1;
        PressAnyKeyText.DOFade(0f, 0.7f).SetLoops(-1, LoopType.Yoyo);
    }

    void Update()
    {
        if (_isLoading) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.anyKey)
        {
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
}

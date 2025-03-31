using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixer SFXMixer;
    public AudioMixer MusicMixer;
    private bool _isLoading;
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        }

        SFXMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        MusicMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);

        FadeManager.Instance.FadeIn();
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

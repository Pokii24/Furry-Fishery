using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    public CanvasGroup fadeImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void FadeIn()
    {
        fadeImage.alpha = 1;
        fadeImage.DOFade(0, 0.5f);
    }

    public void FadeOut()
    {
        fadeImage.alpha = 0;
        fadeImage.DOFade(1, 0.5f);
    }
}
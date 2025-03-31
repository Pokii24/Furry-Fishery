using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerWinOptions : MonoBehaviour
{
    public static PlayerWinOptions Instance;
    public CanvasGroup optionsMenu;
    public RectTransform seeScoreButton;
    public RectTransform playAgainButton;
    public RectTransform quitButton;
    public GameObject scoreMenu;
    public TMP_Text timeLeftText;
    public TMP_Text fishLostMisclicksText;
    public ScoreScriptableObject scoreScriptableObject;
    
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
        timeLeftText.text = $"Time Spared: {TimeSpan.FromSeconds(scoreScriptableObject.timeLeft).ToString(@"mm\:ss")}";
        fishLostMisclicksText.text = $"Fish Lost: {scoreScriptableObject.fishLost}\nMisclicks: {scoreScriptableObject.misclicks}";
    }

    public void ShowWinOptions()
    {
        optionsMenu.alpha = 0;
        optionsMenu.interactable = false;
        optionsMenu.blocksRaycasts = false;
        scoreMenu.SetActive(true);
    }

    public void PlayAgain()
    {
        StartCoroutine(PlayAgainCoroutine());
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator PauseCoroutine()
    {
        optionsMenu.interactable = false;
        optionsMenu.blocksRaycasts = false;
        seeScoreButton.anchoredPosition = new Vector2(0, 318);
        playAgainButton.anchoredPosition = new Vector2(0, 318);
        quitButton.anchoredPosition = new Vector2(0, 318);
        seeScoreButton.DOAnchorPosY(0, 0.75f);
        playAgainButton.DOAnchorPosY(-318, 0.75f);
        quitButton.DOAnchorPosY(-636, 0.75f);
        yield return new WaitForSeconds(0.75f);
        optionsMenu.interactable = true;
        optionsMenu.blocksRaycasts = true;
    }

    public void BackToOptions()
    {
        scoreMenu.SetActive(false);
        optionsMenu.interactable = true;
        optionsMenu.blocksRaycasts = true;
        optionsMenu.alpha = 1;
    }

    IEnumerator PlayAgainCoroutine()
    {
        optionsMenu.interactable = false;
        optionsMenu.blocksRaycasts = false;
        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Scene 0");
    }
}

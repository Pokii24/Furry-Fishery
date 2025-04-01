using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TutorialPlayerController : MonoBehaviour
{
    public List<DialogueScriptableObject> tutorialDialogues = new List<DialogueScriptableObject>();
    public TMP_Text scoreText;
    public int score;
    public bool threeFishCaught;
    public float minFishTime;
    public float maxFishTime;
    private float _currentFishtime;
    private bool _currentlyFishing;
    public AudioSource winGameSound;
    public AudioSource catchFishSound;
    public AudioSource breakLineSound;
    public AudioSource loseFishSound;
    public AudioSource reelingInSound;
    public AudioSource backgroundMusicSound;
    private bool _fishAppear;
    public float startOfReelingTime;
    private float _currentReelingTime;
    public float daylightTimer;
    private float _currentDaylightTimer;
    public Image daylightTimerFill;
    private TutorialStep _currentStep = TutorialStep.CatchThreeFish;
    private bool _currentlySpeaking;
    private bool _loseFishDelay;

    public GameObject ohNoAnim;
    public GameObject niceAnim;
    public GameObject catchAnim;
    public GameObject upRodAnim;
    public GameObject tightLineAnim;
    public GameObject bobberAnim;
    public GameObject defaultRodAnim;
    public GameObject looseLineAnim;
    public GameObject catchFishAnim;
    public GameObject loseFishAnim;
    
    public GameObject dialogueBox;
    public TMP_Text dialogueNameLeft;
    public TMP_Text dialogueNameRight;
    public TMP_Text dialogueText;
    public RectTransform dialogueBoxRect;
    public SpriteRenderer dialogueSprite;
    public SpriteRenderer dialogueSpriteBack;
    public AudioSource dialogueAudio;
    private bool _whisper;
    private bool _isTalking;
    private bool _skipDialogue;
    
    private enum TutorialStep
    {
        CatchThreeFish,
        LoseOneFish,
        PauseMenu,
        LoseWinSystem
    }
    
    void Start()
    {
        _currentlyFishing = true;
        _fishAppear = false;
        _currentFishtime = Random.Range(minFishTime, maxFishTime);
        startOfReelingTime = 999999;
        _currentDaylightTimer = daylightTimer;
        StartCoroutine(NewTutorialDialogue(tutorialDialogues[0]));
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isTalking && PlayerPrefs.HasKey("Tutorial"))
        {
            _skipDialogue = true;
        }
        
        //number countdown until fish appears
        if (_currentlyFishing && !_fishAppear && !PauseMenu.Instance.isPaused && !_currentlySpeaking && _currentStep == TutorialStep.CatchThreeFish)
        {
            _currentFishtime -= Time.deltaTime;
            if (_currentFishtime <= 0)
            {
                _currentlyFishing = false;
                _fishAppear = true;
                _currentReelingTime = startOfReelingTime;
                reelingInSound.Play();
                catchAnim.SetActive(true);
                upRodAnim.SetActive(true);
                tightLineAnim.SetActive(true);
                bobberAnim.SetActive(false);
                defaultRodAnim.SetActive(false);
                looseLineAnim.SetActive(false);
                Debug.Log("fishAppear");
            }
        }
        // if we don't catch fish in time we restart fishing time, resetting fish time
        else if (_fishAppear && !PauseMenu.Instance.isPaused)
        {
            _currentReelingTime -= Time.deltaTime; //turning currentReelingTime into countdown
            if (_currentReelingTime <= 0)
            {
                _currentlyFishing = true;
                _fishAppear = false;
                _currentFishtime = Random.Range(minFishTime, maxFishTime);
               //making sure you can't go into negatives and plays sound
                if (score > 0) score--;
                StartCoroutine(LoseFishAnimation());
                Debug.Log("Fish lost :[");
                reelingInSound.Stop();
                breakLineSound.Play();
                loseFishSound.PlayDelayed(0.4f);
            }
        }
        
        // checks for player input and if 3 fish aren't caught it will increase counter vice versa
        if (Input.GetMouseButtonDown(1) && !threeFishCaught && _fishAppear && !PauseMenu.Instance.isPaused && !_currentlySpeaking && _currentStep == TutorialStep.CatchThreeFish)
        {
            reelingInSound.Stop();
            score++;
            StartCoroutine(CatchAnimation());
            catchFishSound.Play();
            _currentlyFishing = true;
            _fishAppear = false;
            _currentFishtime = Random.Range(minFishTime, maxFishTime);
            if (score >= 3)
            {
                threeFishCaught = true;
                //do next tutorial objective
                Debug.Log("You won!!");
                _currentlyFishing = false;
                backgroundMusicSound.Stop();
                winGameSound.PlayDelayed(0.5f);
            }
        } 
        
        //trying to catch fish when score is more than 0 and there is no fish to catch minus one fish
        else if (Input.GetMouseButtonDown(1) && !_fishAppear && score > 0 && !PauseMenu.Instance.isPaused && !_currentlySpeaking && _currentStep == TutorialStep.LoseOneFish && !_loseFishDelay)
        {
            score--;
            StartCoroutine(LoseFishDelay());
            Debug.Log("Fish lost :[");
            breakLineSound.Play();
        }

        if (!PauseMenu.Instance.isPaused && !threeFishCaught && !_currentlySpeaking)
        {
            //tween sun in correlation to daylightTimer
            _currentDaylightTimer -= Time.deltaTime;
            daylightTimerFill.fillAmount = _currentDaylightTimer / daylightTimer;
            if (_currentDaylightTimer <= 0)
            {
                //lose animation
                //SceneManager.LoadScene("Scene 4");
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !_currentlySpeaking)
        {
            if (_currentStep == TutorialStep.PauseMenu && PauseMenu.Instance.isPaused && !PauseMenu.Instance.isPausing)
            {
                _currentStep = TutorialStep.LoseWinSystem;
                StartCoroutine(NewTutorialDialogue(tutorialDialogues[3]));
            }
            PauseMenu.Instance.PauseGame();
        }
        
        // updates text UI of fish caught
        UpdateObjectiveText();
    }
    //wait before playing catch fish animation 
    IEnumerator CatchAnimation()
    {
        catchAnim.SetActive(false);
        tightLineAnim.SetActive(false);
        catchFishAnim.SetActive(true);
        upRodAnim.SetActive(true);
        niceAnim.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        catchFishAnim.SetActive(false);
        upRodAnim.SetActive(false);
        niceAnim.SetActive(false);
        bobberAnim.SetActive(true);
        defaultRodAnim.SetActive(true);
        looseLineAnim.SetActive(true);
        if (threeFishCaught)
        {
            _currentStep = TutorialStep.LoseOneFish;
            StartCoroutine(NewTutorialDialogue(tutorialDialogues[1]));
        }
    }

    IEnumerator LoseFishAnimation()
    {
        catchAnim.SetActive(false);
        tightLineAnim.SetActive(false);
        loseFishAnim.SetActive(true);
        upRodAnim.SetActive(true);
        ohNoAnim.SetActive(true); 
        yield return new WaitForSeconds(1f);
        loseFishAnim.SetActive(false);
        upRodAnim.SetActive(false);
        ohNoAnim.SetActive(false);
        bobberAnim.SetActive(true);
        defaultRodAnim.SetActive(true);
        looseLineAnim.SetActive(true);
    }

    //update hanging sign
    void UpdateObjectiveText()
    {
        switch (_currentStep)
        {
            case TutorialStep.CatchThreeFish:
                scoreText.text = $"Fish : {score}/3";
                break;
            case TutorialStep.LoseOneFish:
                scoreText.text = $"Fish : {score}/4";
                break;
            case TutorialStep.PauseMenu:
                scoreText.text = "Open pause menu";
                break;
            case TutorialStep.LoseWinSystem:
                break;
        }
    }

    //setting up dialogue system for tutorial
    IEnumerator NewTutorialDialogue(DialogueScriptableObject dialogue)
    {
        _currentlySpeaking = true;

        if (_currentStep == TutorialStep.CatchThreeFish)
        {
            FadeManager.Instance.FadeIn();
            dialogueSprite.sprite = dialogue.dialogueList[0].texture;
            if (dialogue.dialogueList[0].dialogueSpriteBack)
            {
                dialogueSpriteBack.sprite = dialogue.dialogueList[0].dialogueSpriteBack;
            }
            yield return new WaitForSeconds(1f);
        }
        
        if (_currentStep == TutorialStep.LoseWinSystem)
        {
            yield return new WaitForSeconds(1f);
        }
        
        dialogueBox.SetActive(true);
        foreach (Dialogue currentDialogue in dialogue.dialogueList)
        {
            dialogueNameLeft.text = currentDialogue.name;
            dialogueNameRight.text = currentDialogue.name;
            dialogueSprite.sprite = currentDialogue.texture;
            dialogueAudio.clip = currentDialogue.talkSound;
            dialogueText.text = "";
            if (currentDialogue.dialogueSpriteBack != null)
            {
                dialogueSpriteBack.sprite = currentDialogue.dialogueSpriteBack;
            }
            else
            {
                dialogueSpriteBack.sprite = null;
            }
            //based on the character position (left or right), set the textbox width so it doesn't clip into the character
            if (currentDialogue.isLeft)
            {
                dialogueBoxRect.offsetMin = new Vector2(currentDialogue.textWidth, dialogueBoxRect.anchoredPosition.y);
                dialogueBoxRect.offsetMax = new Vector2(-25, dialogueBoxRect.anchoredPosition.y);
                dialogueNameLeft.gameObject.SetActive(false);
                dialogueNameRight.gameObject.SetActive(true);
                dialogueSprite.transform.position = new Vector2(-1.74f, 0);
            }
            else
            {
                dialogueBoxRect.offsetMax = new Vector2(-currentDialogue.textWidth, dialogueBoxRect.anchoredPosition.y);
                dialogueBoxRect.offsetMin = new Vector2(25, dialogueBoxRect.anchoredPosition.y);
                dialogueNameLeft.gameObject.SetActive(true);
                dialogueNameRight.gameObject.SetActive(false);
                dialogueSprite.transform.position = new Vector2(10, 0);
            }
            _isTalking = true;
            foreach (char currentCharacter in currentDialogue.message)
            {
                if (_skipDialogue)
                {
                    dialogueText.text = currentDialogue.message;
                    _skipDialogue = false;
                    break;
                }
                dialogueText.text += currentCharacter;
                if (_whisper)
                {
                    dialogueSprite.sprite = dialogueSprite.sprite;
                    if (currentCharacter == ')')
                    {
                        _whisper = false;
                    }
                }
                else if (currentCharacter == '(')
                {
                    _whisper = true;
                    dialogueSprite.sprite = dialogueSprite.sprite;
                }
                else if (!Char.IsLetterOrDigit(currentCharacter))
                {
                    dialogueSprite.sprite = currentDialogue.texture;
                }
                else
                {
                    dialogueSprite.sprite = currentDialogue.textureTalking;
                    dialogueAudio.Play();
                }
                yield return new WaitForSeconds(0.1f);
            }
            _isTalking = false;
            dialogueSprite.sprite = currentDialogue.texture;
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }
        }
        dialogueBox.SetActive(false);
        dialogueSprite.sprite = null;
        dialogueSpriteBack.sprite = null;
        if (_currentStep == TutorialStep.LoseWinSystem)
        {
            PlayerPrefs.SetInt("Tutorial", 1);
            PlayerPrefs.Save();
            FadeManager.Instance.FadeOut();
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Scene 3");
        }
        else
        {
            _currentlySpeaking = false;
        }
    }

    public void PauseMenuResumeButton()
    {
        if (_currentStep == TutorialStep.PauseMenu)
        {
            _currentStep = TutorialStep.LoseWinSystem;
            StartCoroutine(NewTutorialDialogue(tutorialDialogues[3]));
        }
        PauseMenu.Instance.PauseGame();
    }

    IEnumerator LoseFishDelay()
    {
        _loseFishDelay = true;
        yield return new WaitForSeconds(1f);
        _currentStep = TutorialStep.PauseMenu;
        StartCoroutine(NewTutorialDialogue(tutorialDialogues[2]));
    }
}
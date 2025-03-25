using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    }
    
    void Update()
    {
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
                _currentStep = TutorialStep.LoseOneFish;
                threeFishCaught = true;
                //do next tutorial objective
                Debug.Log("You won!!");
                _currentlyFishing = false;
                backgroundMusicSound.Stop();
                winGameSound.PlayDelayed(0.5f);
            }
        } 
        else if (Input.GetMouseButtonDown(1) && threeFishCaught && !PauseMenu.Instance.isPaused)
        {
            Debug.Log("Stop catching fish");
        }
        //trying to catch fish when score is more than 0 and there is no fish to catch minus one fish
        else if (Input.GetMouseButtonDown(1) && !_fishAppear && score > 0 && !PauseMenu.Instance.isPaused && !_currentlySpeaking && _currentStep == TutorialStep.LoseOneFish)
        {
            score--;
            _currentStep = TutorialStep.PauseMenu;
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
            if (_currentStep == TutorialStep.PauseMenu)
            {
                _currentStep = TutorialStep.LoseWinSystem;
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
            yield return new WaitForSeconds(2f);
            if (LevelSystem.Instance.level == 5)
            {
                SceneManager.LoadScene("Scene 6");
            }
            else
            {
                SceneManager.LoadScene("Scene 5");
            }
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

    void UpdateObjectiveText()
    {
        switch (_currentStep)
        {
            case TutorialStep.CatchThreeFish:
                scoreText.text = $"Fish : {score}/3";
                break;
            case TutorialStep.LoseOneFish:
                scoreText.text = "Lose one fish";
                break;
            case TutorialStep.PauseMenu:
                scoreText.text = "Open pause menu";
                break;
            case TutorialStep.LoseWinSystem:
                break;
        }
    }
}

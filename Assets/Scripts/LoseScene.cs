using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoseScene : MonoBehaviour
{
    public DialogueScriptableObject loseDialogue;
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
    
    private IEnumerator Start()
    {
        //fade the screen in amd set the sprites correlating to who is going to speak next
        FadeManager.Instance.FadeIn();
        dialogueSprite.sprite = loseDialogue.dialogueList[0].texture;
        if (loseDialogue.dialogueList[0].dialogueSpriteBack)
        {
            dialogueSpriteBack.sprite = loseDialogue.dialogueList[0].dialogueSpriteBack;
        }
        //wait for the screen to fade in and then make dialogue box visible
        yield return new WaitForSeconds(1f);
        dialogueBox.SetActive(true);
        foreach (Dialogue currentDialogue in loseDialogue.dialogueList)
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
            }
            else
            {
                dialogueBoxRect.offsetMax = new Vector2(-currentDialogue.textWidth, dialogueBoxRect.anchoredPosition.y);
                dialogueBoxRect.offsetMin = new Vector2(25, dialogueBoxRect.anchoredPosition.y);
                dialogueNameLeft.gameObject.SetActive(true);
                dialogueNameRight.gameObject.SetActive(false);
            }
            //run a foreach loop for every character (not person, letter/numbers) in the message
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


            dialogueSprite.sprite = currentDialogue.texture;
            //wait to go to the next dialogue, yield return null to tell unity script is being used
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }
        }
        dialogueBox.SetActive(false);
        dialogueSprite.sprite = null;
        dialogueSpriteBack.sprite = null;
        //wait for fadeout
        LevelSystem.Instance.level = 1;
        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Scene 0");
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isTalking)
        {
            _skipDialogue = true;
        }
    }
}

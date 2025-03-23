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
    
    private IEnumerator Start()
    {
        //wait for screen to fade in
        dialogueBox.SetActive(true);
        foreach (Dialogue currentDialogue in loseDialogue.dialogues)
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
            //based on the character position (left or right), set the textbox width so it doesnt clip into the character
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
            foreach (char currentCharacter in currentDialogue.message)
            {
                dialogueText.text += currentCharacter;
                if (currentCharacter == ' ')
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
        SceneManager.LoadScene("Scene 0");
    }
}

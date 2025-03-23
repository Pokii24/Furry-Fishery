using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class DialogueScriptableObject : ScriptableObject
{
    public List<Dialogue> dialogues = new List<Dialogue>();
}

[System.Serializable]
public struct Dialogue
{
    public string name;
    public string message;
    public bool isLeft;
    public float textWidth;
    public AudioClip talkSound;
    public Sprite texture;
    public Sprite textureTalking;
    public Sprite dialogueSpriteBack;
}

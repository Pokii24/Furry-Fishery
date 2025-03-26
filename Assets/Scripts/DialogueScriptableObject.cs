using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class DialogueScriptableObject : ScriptableObject
{
    [FormerlySerializedAs("dialogues")] public List<Dialogue> dialogueList = new List<Dialogue>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreScriptableObject", menuName = "ScoreScriptableObject")]
public class ScoreScriptableObject : ScriptableObject
{
    public float timeLeft;
    public int fishLost;
    public int misclicks;

    public void ResetScore()
    {
        timeLeft = 0;
        fishLost = 0;
        misclicks = 0;
    }
}
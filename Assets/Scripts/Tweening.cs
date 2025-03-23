using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tweening : MonoBehaviour
{
    public RectTransform hangingSign;
    
    void Start()
    {
        hangingSign.anchoredPosition = new Vector2(hangingSign.anchoredPosition.x, 324);
        hangingSign.DOAnchorPosY(0, 1);
    }
 }
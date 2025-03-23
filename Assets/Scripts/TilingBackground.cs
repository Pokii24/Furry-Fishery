using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilingBackground : MonoBehaviour
{

    public float speed;
    public bool reverse;
    private Vector3 startPosition;
    private float repeatWidth;
    
    void Start()
    {
        startPosition = transform.position;
        repeatWidth = GetComponent<BoxCollider2D>().size.x / 2;
    }

    
    void Update()
    {
        if (reverse)
        {
            transform.Translate(Vector3.left * (speed * Time.deltaTime));
        
            if (transform.position.x < startPosition.x - repeatWidth)
            {
                transform.position = startPosition;
            }
        }
        else
        {
            transform.Translate(Vector3.right * (speed * Time.deltaTime));
        
            if (transform.position.x > startPosition.x + repeatWidth)
            {
                transform.position = startPosition;
            }
        }
    }
}
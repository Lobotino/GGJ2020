using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite brokenSprite, goodSprite;

    public void setBrokenState(bool state)
    {
        spriteRenderer.sprite = state ? brokenSprite : goodSprite;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

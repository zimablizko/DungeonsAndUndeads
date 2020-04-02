using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RockAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Health health;
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite lightDamagedSprite;
    [SerializeField] private Sprite hardDamagedSprite;
    private int hp100;
    private int hp60;
    private int hp30;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        hp100 = health.MaxHealth;
        hp60 = (int)(health.MaxHealth*0.6);
        hp30 = (int)(health.MaxHealth*0.3);
        spriteRenderer.sprite = fullSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.CurrentHealth <= hp60 && spriteRenderer.sprite == fullSprite)
        {
            spriteRenderer.sprite = lightDamagedSprite;
        }
        if (health.CurrentHealth <= hp30 && spriteRenderer.sprite != hardDamagedSprite)
        {
            spriteRenderer.sprite = hardDamagedSprite;
        }
            
    }
}

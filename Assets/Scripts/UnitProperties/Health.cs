using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;
    public int CurrentHealth
    {
        get { return health; }
    }
    [SerializeField] private int maxHealth;
    public int MaxHealth
    {
        get { return maxHealth; }
    }

        private IObjectDestroyer destroyer;
    
    private void Start()
    {
        GameManager.Instance.healthContainer.Add(gameObject,this);
    }

    public void Init(IObjectDestroyer destroyer, int maxHp)
    {
        this.destroyer = destroyer;
        SetHealth(maxHp);
    }
    public void TakeHit(int damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            if (destroyer == null)
                Destroy(gameObject);
            else destroyer.Destroy(gameObject);
        }
    }

    public void AddHealth(int bonusHealth)
    {
        health += bonusHealth;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }    
    public void AddMaxHealth(int bonusHealth)
    {
        maxHealth += bonusHealth;
        health += bonusHealth;
    }    
    
    public void SetHealth(int bonusHealth)
    {
        maxHealth = bonusHealth;
        health = bonusHealth;
    }
}

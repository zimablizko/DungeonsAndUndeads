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
    [SerializeField] private GameObject particleHitPrefab;
    [SerializeField] private float particleEmitShiftY = 0f;

    private void Start()
    {
        GameManager.Instance.healthContainer.Add(gameObject, this);
    }

    public void Init(IObjectDestroyer destroyer, int maxHp)
    {
        this.destroyer = destroyer;
        SetHealth(maxHp);
    }

    public void TakeHit(int damage)
    {
        health = Math.Max(health - damage, 0);
        if (particleHitPrefab)
        {
            GameObject particle;
            particle = Instantiate(particleHitPrefab,
                new Vector3(transform.position.x, transform.position.y + particleEmitShiftY, transform.position.z),
                Quaternion.identity);
            particle.GetComponent<ParticleSystem>().Emit(20);
        }

        //health = Math.Max(health, 0);

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

    public void SetFullHealth()
    {
        Debug.Log(health);
        health = maxHealth;
    }
}
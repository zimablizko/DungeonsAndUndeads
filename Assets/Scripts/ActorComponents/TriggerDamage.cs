using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private bool destroySelfAfterCollision;
    private string soundHitName;

    private IObjectDestroyer destroyer;
    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public void Init(IObjectDestroyer destroyer, int damage, string soundHitName)
    {
        this.destroyer = destroyer;
        this.damage = damage;
        this.soundHitName = soundHitName;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.actorsContainer.ContainsKey(col.gameObject))
        {
            var actor = GameManager.Instance.actorsContainer[col.gameObject];
            actor.TakeHit(damage);
            if (soundHitName != null)
                AudioManager.Instance.Play(soundHitName);
        }

        if (destroySelfAfterCollision)
        {
            if (destroyer == null)
                Destroy(gameObject);
            else destroyer.Destroy(gameObject);
        }
    }
}



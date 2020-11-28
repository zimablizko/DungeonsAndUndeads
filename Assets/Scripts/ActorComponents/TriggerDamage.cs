using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private bool destroySelfAfterCollision;

    private IObjectDestroyer destroyer;
    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public void Init(IObjectDestroyer destroyer, int damage)
    {
        this.destroyer = destroyer;
        this.damage = damage;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.actorsContainer.ContainsKey(col.gameObject))
        {
            var actor = GameManager.Instance.actorsContainer[col.gameObject];
           
            actor.TakeHit();
            actor.Health.TakeHit(damage);
            GFXManager.Instance.CreateFloatingText(actor.transform, damage.ToString());
        }

        if (destroySelfAfterCollision)
        {
            if (destroyer == null)
                Destroy(gameObject);
            else destroyer.Destroy(gameObject);
        }
    }
}



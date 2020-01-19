using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private bool isDestroyingAfterCollision;

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
        if (GameManager.Instance.healthContainer.ContainsKey(col.gameObject))
        {
            var health = GameManager.Instance.healthContainer[col.gameObject];
            health.TakeHit(damage);
        }

        if (isDestroyingAfterCollision)
        {
            if (destroyer == null)
                Destroy(gameObject);
            else destroyer.Destroy(gameObject);
        }
    }
}

public interface IObjectDestroyer
{
    void Destroy(GameObject gameObject);
}

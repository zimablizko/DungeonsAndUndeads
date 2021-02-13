using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float force;
    [SerializeField] private float lifeTime;
    [SerializeField] private TriggerDamage triggerDamage;
    [SerializeField] private int damage;
    [SerializeField] private string soundHitName;
    private Actor actor;
    
    public float Force
    {
        get => force;
        set => force = value;
    }

    public void SetImpulse(Vector2 direction, Actor actor, int damage)
    {
        if (actor)
            this.actor = actor;
        triggerDamage.Init(this,damage,soundHitName);
        Debug.Log(direction);
        rigidbody.AddForce(direction*force, ForceMode2D.Impulse);
        transform.rotation = Quaternion.Euler(0,direction.x < 0 ? 180 : 0,0);
        StartCoroutine(StartLife());
    }

    private IEnumerator StartLife()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void Destroy(GameObject gameeObject)
    {
        if (actor)
            actor.ReturnProjectileToPool(this);
        else
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float force;
    [SerializeField] private float lifeTime;
    [SerializeField] private TriggerDamage triggerDamage;
    [SerializeField] private int damage;
    private Player player;
    
    public float Force
    {
        get => force;
        set => force = value;
    }

    public void SetImpulse(Vector2 direction, Player player, int damage)
    {
        this.player = player;
        triggerDamage.Init(this,damage);
        rigidbody.AddForce(direction*force, ForceMode2D.Impulse);
        transform.rotation = Quaternion.Euler(0,direction.x < 0 ? 180 : 0,0);
        StartCoroutine(StartLife());
    }

    private IEnumerator StartLife()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void Destroy(GameObject gameObject)
    {
        player.ReturnFireballToPool(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int pushForce = 10;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private bool destroySelfAfterCollision = false;
    [SerializeField] private GameObject particlePrefab;
    private Health health;
    private float direction;
    private bool isCooldown;
    public float Direction => direction;

    private void OnCollisionStay2D(Collision2D col)
    {

            if (GameManager.Instance.healthContainer.ContainsKey(col.gameObject))
            {
                health = GameManager.Instance.healthContainer[col.gameObject];
                if (animator != null) //если это враг или что-то с анимацией
                {
                    direction = (col.transform.position - transform.position).x;
                    animator.SetFloat("Direction", Mathf.Abs(direction));
                }
                else //если это статичное препятствие
                {
                    if (health != null && !isCooldown)
                    {
                        var force = transform.position - col.transform.position;
                        Player.Instance.TakeHit(damage);
                        force.Normalize ();
                        force.y = Mathf.Abs(force.y);
                        force.x *= -0.75f;
                        GameManager.Instance.rigidbodyContainer[col.gameObject].AddForce(force *pushForce, ForceMode2D.Impulse);
                        StartCoroutine(StartCooldown());
                    }

                    health = null;
                }
                if (destroySelfAfterCollision)
                {
                    if (particlePrefab)
                    {
                        GameObject particle;
                        particle = Instantiate(particlePrefab, new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
                        particle.GetComponent<ParticleSystem>().Emit(20);
                    }
                    Destroy(gameObject);
                }
            }
    }

    public void SetDamage()
    {
        if (health != null)
            health.TakeHit(damage);
        health = null;
        direction = 0f;
        animator.SetFloat("Direction", 0f);
    }
    
    private IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(0.3f);
        isCooldown = false;
    }
}


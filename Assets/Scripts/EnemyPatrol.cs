using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject leftBorder;
    [SerializeField] private GameObject rightBorder;
    [SerializeField] private Rigidbody2D rigitbody;
    [SerializeField] private GroundDetection groundDetection;
    [SerializeField] private CollisionDamage collisionDamage;
    [SerializeField] private bool isRightDirection;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float speed = 1f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Animator animator;

    private void Start()
    {
        GameManager.Instance.rigidbodyContainer.Add(gameObject,rigitbody);
    }

    private void Update()
    {
        if (groundDetection.IsGrounded)
        {
            if (transform.position.x > rightBorder.transform.position.x || collisionDamage.Direction < 0)
                isRightDirection = false;
            else if (transform.position.x < leftBorder.transform.position.x || collisionDamage.Direction > 0) 
                isRightDirection = true;
            rigitbody.velocity = isRightDirection ? Vector2.right : Vector2.left;
            rigitbody.velocity *= speed;
        }
        if (rigitbody.velocity.x > 0)
            spriteRenderer.flipX = false;
        if (rigitbody.velocity.x < 0)
            spriteRenderer.flipX = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingBehaviour : StateMachineBehaviour
{
    private float speed;
    public float attackRange = 1.5f;

    private Transform player;
    private Rigidbody2D rb;
    private Enemy enemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();
        speed = enemy.Speed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!player.GetComponent<Player>().isActiveAndEnabled)
        {
            animator.Play("Idle");
            return;
        }
        enemy.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        
       if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            enemy.MeleeAttack();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger("StartAttackMelee");
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossIdleBehaviour : StateMachineBehaviour
{
    public float attackRange = 1.8f;
    public bool enrageMode = false;
    public int shootCounter = 0;
    private Transform player;
    private Rigidbody2D rb;

    private Enemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemy.Health.CurrentHealth <= (enemy.Health.MaxHealth / 2))
        {
            if (!enrageMode)
            {
                enrageMode = true;
                animator.SetTrigger("StartSplit");
            }
        }

        if (!player.GetComponent<Player>().isActiveAndEnabled)
        {
            animator.Play("Idle");
            return;
        }

        enemy.LookAtPlayer();
        if (Vector2.Distance(player.position, rb.position) > attackRange)
        {
            animator.SetTrigger("StartWalking");
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var rnd = Random.Range(0, 3);
        if (rnd != 1)
        {
            enemy.CheckShoot();
        }
        else
        {
            animator.SetTrigger("StartCast");
        }
        
        animator.ResetTrigger("StartWalking");
        //animator.ResetTrigger("StartAttackMelee");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
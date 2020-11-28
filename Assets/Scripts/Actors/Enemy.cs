using UnityEngine;

public class Enemy : Actor
{
    private GameObject player;

    void Start()
    {
        base.Start();
        player = GameManager.Instance.player.gameObject;
    }

    
    void FixedUpdate()
    {
        //MovementUpdate(Vector3.left);
        //MeleeAttack();


        //animator.SetFloat("Speed", Mathf.Abs(rigidbody.velocity.x));
    }

    public void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - (Vector3) rigidbody.position;
        
        if (direction.x > 0)
        {
            isOnRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (direction.x < 0)
        {
            isOnRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
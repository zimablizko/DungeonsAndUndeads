using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour, IObjectDestroyer
{
    
    [Header("COMPONENTS")] 
    [SerializeField] public Rigidbody2D rigidbody;
    [SerializeField] public Animator animator;
    [SerializeField] private Health health;

    public Health Health
    {
        get { return health; }
    }

    [SerializeField] private GroundDetection groundDetection;

    [Header("VARIABLES")] 
    [SerializeField] private int initHealth = 100;
    [SerializeField] private float jumpForce = 10;

    public float Force
    {
        get => jumpForce;
        set => jumpForce = value;
    }
    [SerializeField] private float actionSpeed = 0.5f;
    [SerializeField] private float speed = 3;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    
    [SerializeField] private float recoveryTime = 0.5f;
    [Header("FLAGS")]
    [SerializeField] private bool isDisabled;

    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }

    [SerializeField] private bool isJumping;
    [SerializeField] private bool isCooldown;
    [SerializeField] private bool isMovable = true;
    [SerializeField] public bool isOnRight = true;
    
    [Header("MELEE COMBAT")]
    [SerializeField] private int meleeDamage = 5;
    [SerializeField] private float meleeAttackRate = 2f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private Transform attackPoint;
    private float nextMeleeAttackTime;
    private float meleeDamageBonus;
    private float meleeDamageMultiplier;
    public int MeleeDamage
    {
        get => (int)((meleeDamage + (int) meleeDamageBonus) * (1 + meleeDamageMultiplier));
        set => meleeDamage = value;
    }
    
    [Header("RANGE COMBAT")]
    [SerializeField] private int rangeDamage = 5;
    [SerializeField] private float rangeAttackRate = 2f;
    [SerializeField] private Projectile projectile;
    private float nextRangeAttackTime;
    private float rangeDamageBonus;
    private float rangeDamageMultiplier;
    [SerializeField] private Transform projectileSpawnPoint;
    private int projectileCount = 5;
    private Projectile currentProjectile;
    private List<Projectile> projectilePool;
    public int RangeDamage
    {
        get => (int)((rangeDamage + (int) rangeDamageBonus) * (1 + rangeDamageMultiplier));
        set => rangeDamage = value;
    }
    
    [Header("OTHER")]
    
    [SerializeField] private LayerMask enemyLayers;
    
    private Vector3 direction;
    
    private float jumpForceBonus;
    private float healthBonus;
    
    public BuffReceiver buffReceiver;


    public void Start()
    {
        if (projectile)
        {
            projectilePool = new List<Projectile>();
            for (int i = 0; i < projectileCount; i++)
            {
                var projectileTemp = Instantiate(projectile, projectileSpawnPoint);
                projectilePool.Add(projectileTemp);
                projectileTemp.gameObject.SetActive(false);
            }
        }

        GameManager.Instance.rigidbodyContainer.Add(gameObject, rigidbody);
        buffReceiver.OnBuffsChanged += BuffUpdate;
        health.Init(this, initHealth + (int) healthBonus);
        ActorInit();
    }

    public void ActorInit()
    {
        //rigidbody.bodyType = RigidbodyType2D.Dynamic;
        GameManager.Instance.actorsContainer.Add(gameObject, this);
        animator.SetBool("IsDead", false);
        isDisabled = false;
        isMovable = true;
        direction = Vector3.zero;
    }

    private void BuffUpdate()
    {
        // Melee Damage
        foreach (var buff in buffReceiver.Buffs.FindAll(buff => buff.type == BuffType.MeleeDamage))
        {
            meleeDamageBonus += buff.additiveBonus != 0 ? buff.additiveBonus : 0;
            meleeDamageMultiplier += buff.multipleBonus != 0 ? buff.multipleBonus : 0;
        }
        // Range Damage
        foreach (var buff in buffReceiver.Buffs.FindAll(buff => buff.type == BuffType.RangeDamage))
        {
            rangeDamageBonus += buff.additiveBonus != 0 ? buff.additiveBonus : 0;
            rangeDamageMultiplier += buff.multipleBonus != 0 ? buff.multipleBonus : 0;
        }
        // Health
        foreach (var buff in buffReceiver.Buffs.FindAll(buff => buff.type == BuffType.Health))
        {
            healthBonus += buff.additiveBonus != 0 ? buff.additiveBonus : 0;
        }
        health.SetHealth(initHealth + (int) healthBonus);
    }

    void FixedUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidbody.velocity.x));
    }

    private void Update()
    {

    }

    public void MovementUpdate(Vector3 moveDirection)
    {
        animator.SetBool("isGrounded", groundDetection.IsGrounded);
        if (!isJumping && !groundDetection.IsGrounded)
        {
            animator.SetTrigger("StartFall");
        }

        isJumping = /*isJumping &&*/ !groundDetection.IsGrounded;
        direction = Vector3.zero;
        if (isMovable)
        {
            direction = moveDirection;
        }
        direction *= speed;
        direction.y = rigidbody.velocity.y;
        if (!isDisabled)
            rigidbody.velocity = direction;
        
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
    /* private void OnCollisionStay2D(Collision2D collision)
      {
          if (rigitbody.velocity.y > 0 && !isJumping)
          {
              direction.y = 0;
              rigitbody.velocity = direction;
          }
      }*/

    public void Stop()
    {
        MovementUpdate(Vector3.zero);
    }
    public void Jump()
    {
        if (!isCooldown && groundDetection.IsGrounded)
        {
            rigidbody.AddForce(Vector2.up * (jumpForce + jumpForceBonus), ForceMode2D.Impulse);
            animator.SetTrigger("StartJump");
            isMovable = true;
            isJumping = true;
            StartCoroutine(StartCooldown());
        }
    }

    #region MELEE ATTACK

    public void MeleeAttack()
    {
        if (Time.time >= nextMeleeAttackTime && !isJumping)
        {
            isMovable = false;
            animator.SetTrigger("StartAttackMelee");
            nextMeleeAttackTime = Time.time + 1f / meleeAttackRate;
        }
    }

    //Момент анимации атаки, когда наносится урон
    void AttackImpact()
    {
        //meleeAttackRegion.SetActive(true);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (GameManager.Instance.actorsContainer.ContainsKey(enemy.gameObject))
            {
                var actor = GameManager.Instance.actorsContainer[enemy.gameObject];
                actor.TakeHit(MeleeDamage);
            }
        }
    }

    //Последний кадр анимации атаки
    void AttackFinish()
    {
        isMovable = true;
    }

    #endregion

    #region RANGE ATTACK

    public void CheckShoot()
    {
        if (Time.time >= nextRangeAttackTime && !isJumping)
        {
            isMovable = false;
            animator.SetTrigger("StartAttackRange");
            nextRangeAttackTime = Time.time + 1f / rangeAttackRate;
        }
    }

    void Shoot()
    {
        currentProjectile = GetProjectileFromPool();
        currentProjectile.SetImpulse(isOnRight ? Vector2.right : Vector2.left, this, RangeDamage);
        StartCoroutine(StartCooldown());
        isMovable = true;
    }

    private IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(actionSpeed);
        isCooldown = false;
    }

    private Projectile GetProjectileFromPool()
    {
        if (projectilePool.Count > 0)
        {
            var projectileTemp = projectilePool[0];
            projectilePool.Remove(projectileTemp);
            projectileTemp.gameObject.SetActive(true);
            projectileTemp.transform.parent = null;
            projectileTemp.transform.position = projectileSpawnPoint.transform.position;
            return projectileTemp;
        }

        return Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
    }

    public void ReturnProjectileToPool(Projectile projectileTemp)
    {
        if (!projectilePool.Contains(projectileTemp))
            projectilePool.Add(projectileTemp);
        projectileTemp.transform.parent = projectileSpawnPoint;
        projectileTemp.transform.position = projectileSpawnPoint.transform.position;
        projectileTemp.gameObject.SetActive(false);
    }

    #endregion
    
    public void TakeHit(int hitDamage)
    {
        Health.TakeHit(hitDamage);
        GFXManager.Instance.CreateFloatingText(transform, hitDamage.ToString());
        IsDisabled = true;
        if (health.CurrentHealth > 0f)
        {
            animator.SetTrigger("TakeDamage");
            StartCoroutine(RecoverTimeout());
        } else{
            Destroy(gameObject);
        }
    }

    private IEnumerator RecoverTimeout()
    {
        yield return new WaitForSeconds(recoveryTime);
        RecoverFromHit();
    }

    public void RecoverFromHit()
    {
        IsDisabled = false;
    }

    public void Destroy(GameObject gameObject)
    {
        IsDisabled = true;
        animator.SetBool("IsDead", true);
        rigidbody.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
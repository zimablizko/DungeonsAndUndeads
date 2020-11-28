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
    [Header("PREFABS")] 
    [SerializeField] private Projectile projectile;
    [Header("VARIABLES")] 
    [SerializeField] private int initHp = 100;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float jumpForce = 10;

    public float Force
    {
        get => jumpForce;
        set => jumpForce = value;
    }

    [SerializeField] private float speed = 3;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField] private float actionSpeed = 0.5f;
    [SerializeField] private float attackSpeed = 2f;
    private float nextAttackTime = 0f;
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
    [Header("OTHER")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    
    private Vector3 direction;
    [SerializeField] private Transform projectileSpawnPoint;
    private int projectileCount = 5;

    private float damageBonus;
    private float jumpForceBonus;
    private float healthBonus;

    public int Damage
    {
        get => damage + (int) damageBonus;
        set => damage = value;
    }

    public BuffReciever buffReciever;
    private Projectile currentProjectile;
    private List<Projectile> projectilePool;

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
        buffReciever.OnBuffsChanged += BuffUpdate;
        health.Init(this, initHp + (int) healthBonus);
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
        Buff forceBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Force);
        Buff healthBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Armor);
        Buff damageBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Damage);
        jumpForceBonus = forceBuff == null ? 0 : forceBuff.additiveBonus;
        healthBonus = healthBuff == null ? 0 : healthBuff.additiveBonus;
        damageBonus = damageBuff == null ? 0 : damageBuff.additiveBonus;
        health.SetHealth(initHp + (int) healthBonus);
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
        if (Time.time >= nextAttackTime && !isJumping)
        {
            isMovable = false;
            animator.SetTrigger("StartAttackMelee");
            nextAttackTime = Time.time + 1f / attackSpeed;
            //StartCoroutine(StartCooldown());
        }
    }

    //Момент анимации атаки, когда наносится урон
    void AttackImpact()
    {
        //meleeAttackRegion.SetActive(true);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("HIT "+ enemy.name);
            if (GameManager.Instance.actorsContainer.ContainsKey(enemy.gameObject))
            {
                var actor = GameManager.Instance.actorsContainer[enemy.gameObject];
                actor.Health.TakeHit(damage);
                GFXManager.Instance.CreateFloatingText(actor.transform, damage.ToString());
                actor.TakeHit(); //TODO: Переделать эту херню
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
        if (!isCooldown)
        {
            isMovable = false;
            animator.SetTrigger("StartAttackRange");
            
        }
    }

    void Shoot()
    {
        currentProjectile = GetProjectileFromPool();
        currentProjectile.SetImpulse(isOnRight ? Vector2.right : Vector2.left, this, (int) (damage + damageBonus));
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
    
    public void TakeHit()
    {
        IsDisabled = true;
        if (health.CurrentHealth > 0f)
        {
            animator.SetTrigger("TakeDamage");
            StartCoroutine(RecoverTimeout());
        }else{
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
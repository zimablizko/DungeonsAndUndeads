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
    [SerializeField] private Energy energy;

    public Energy Energy
    {
        get { return energy; }
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
    [SerializeField] private float resistTime = 3f;
    [Header("FLAGS")]
    [SerializeField] private bool isInvulnerable;

    public bool IsInvulnerable
    {
        get => isInvulnerable;
        set => isInvulnerable = value;
    }    
    
    [SerializeField] private bool isDisabled;

    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }

    [SerializeField] private bool isJumping;
    [SerializeField] private bool isCooldown;
    [SerializeField] public bool isMovable = true;
    [SerializeField] public bool isOnRight = true;
    [SerializeField] public bool isBlocking;
    [SerializeField] public bool isResisting;
    [SerializeField] public bool isPlayer;

    public bool IsPlayer
    {
        get => isPlayer;
        set => isPlayer = value;
    }

    [Header("MELEE COMBAT")]
    [SerializeField] private int meleeDamage = 5;
    [SerializeField] private float meleeAttackRate = 2f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
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
    [SerializeField] private int rangeEnergyCost = 0;
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

    [Header("SOUNDS")] 
    [SerializeField] private string soundTakeHit;
    [SerializeField] private string soundDeath;
    [SerializeField] private string soundMeleeAttack;
    [SerializeField] private string soundRangeCast;
    [SerializeField] private string soundRangeRelease;
    
    [Header("OTHER")]
    private Vector3 direction;
    private float jumpForceBonus;
    private float healthBonus;
    private float healthMultiplier;
    
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
        health.Init(this, (int)((initHealth + (int) healthBonus) * (1 + healthMultiplier)));
        ActorInit();
    }

    public void ActorInit()
    {
        //rigidbody.bodyType = RigidbodyType2D.Dynamic;
        GameManager.Instance.AddActor(gameObject, this);
        animator.SetBool("IsDead", false);
        isDisabled = false;
        isMovable = true;
        direction = Vector3.zero;
        if (!isPlayer)
        {
            buffReceiver.AddBuff(new Buff(BuffType.Health, 0, 0.1f*(GameManager.Instance.difficulty-1)));
            buffReceiver.AddBuff(new Buff(BuffType.MeleeDamage, 0, 0.1f*(GameManager.Instance.difficulty-1)));
            buffReceiver.AddBuff(new Buff(BuffType.RangeDamage, 0, 0.1f*(GameManager.Instance.difficulty-1)));
        }
    }

    private void BuffUpdate()
    {
        /*
        meleeDamageBonus = 0;
        meleeDamageMultiplier = 0;
        rangeDamageBonus = 0;
        rangeDamageMultiplier = 0;
        healthBonus = 0;
        healthMultiplier = 0;
        */
        
        // Melee Damage
        foreach (var buff in buffReceiver.Buffs)
        {
            if (buff.type == BuffType.MeleeDamage)
            {
                meleeDamageBonus += buff.additiveBonus;
                meleeDamageMultiplier += buff.multipleBonus;
            } else if (buff.type == BuffType.RangeDamage)
            {
                rangeDamageBonus += buff.additiveBonus;
                rangeDamageMultiplier += buff.multipleBonus;
            } else if (buff.type == BuffType.Health)
            {
                healthBonus += buff.additiveBonus;
                healthMultiplier += buff.multipleBonus;
                health.SetHealth((int)((initHealth + (int) healthBonus) * (1 + healthMultiplier)));
            }
        }
        buffReceiver.Buffs.Clear();
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
        if (!isCooldown && groundDetection.IsGrounded && !IsDisabled)
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
        AudioManager.Instance.Play(soundMeleeAttack);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (GameManager.Instance.actorsContainer.ContainsKey(enemy.gameObject))
            {
                var actor = GameManager.Instance.actorsContainer[enemy.gameObject];
                actor.TakeHit(MeleeDamage, gameObject);
                if (isPlayer)
                    Energy.AddEnergy(1);
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
            if (Energy && Energy.CurrentEnergy<rangeEnergyCost)
                return;
            isMovable = false;
            animator.SetTrigger("StartAttackRange");
            nextRangeAttackTime = Time.time + 1f / rangeAttackRate;
            AudioManager.Instance.Play(soundRangeCast);
        }
    }

    void Shoot()
    {
        if (Energy)
            Energy.AddEnergy(-rangeEnergyCost);
        currentProjectile = GetProjectileFromPool();
        currentProjectile.SetImpulse(isOnRight ? Vector2.right : Vector2.left, this, RangeDamage);
        AudioManager.Instance.Play(soundRangeRelease);
        StartCoroutine(StartCooldown());
        isMovable = true;
    }
    
    void ShootToPlayer()
    {
        if (Energy)
            Energy.AddEnergy(-rangeEnergyCost);
        currentProjectile = GetProjectileFromPool();
        Transform target = Player.Instance.transform;
        currentProjectile.SetImpulse(Vector3.Normalize(target.position - transform.position) - new Vector3(0,0.07f,0), this, RangeDamage);
        AudioManager.Instance.Play(soundRangeRelease);
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
    
    public void TakeHit(int hitDamage, GameObject damageSource = null)
    {
        if (IsInvulnerable) return;
        var damageBlocked = false;
        if (damageSource && isBlocking)
            if ((!isOnRight && damageSource.transform.position.x < gameObject.transform.position.x) || 
                (isOnRight && damageSource.transform.position.x > gameObject.transform.position.x))
            {
                damageBlocked = true;
                hitDamage = 1;
            }

        Health.TakeHit(hitDamage);
        rigidbody.velocity = Vector2.zero;
        GFXManager.Instance.CreateFloatingText(transform, hitDamage.ToString());
        AudioManager.Instance.Play(soundTakeHit);
        if (health.CurrentHealth <= 0f)
        {
            AudioManager.Instance.Play(soundDeath);
            Destroy(gameObject);
            return;
        } 
        if (!damageBlocked && !isResisting)
        {
            animator.SetTrigger("TakeDamage");
            IsDisabled = true;
            StartCoroutine(RecoverTimeout());
            isResisting = true;
            StartCoroutine(ResistTimeout());
        }
    }

    private IEnumerator RecoverTimeout()
    {
        yield return new WaitForSeconds(recoveryTime);
        RecoverFromHit();
    }    
    private IEnumerator ResistTimeout()
    {
        yield return new WaitForSeconds(resistTime);
        isResisting = false;
    }

    public void RecoverFromHit()
    {
        IsDisabled = false;
        isMovable = true;
    }

    public void Destroy(GameObject gameObject)
    {
        IsDisabled = true;
        animator.SetBool("IsDead", true);
        rigidbody.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        GameManager.Instance.RemoveActor(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
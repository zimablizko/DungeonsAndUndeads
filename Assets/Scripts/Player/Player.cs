using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, IObjectDestroyer
{
    #region Singleton

    public static Player Instance { get; private set; }

    #endregion

    [SerializeField] private float speed;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    [SerializeField] private float jumpForce;
   
    public float Force
    {
        get => jumpForce;
        set => jumpForce = value;
    }
    [SerializeField] private float attackSpeed;
    [SerializeField] private float recoveryTime;
    [SerializeField] private bool isDisabled;
    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }
    [SerializeField] private Rigidbody2D rigitbody;
    [SerializeField] private GameObject meleeAttackRegion;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isCheatMode;
    [SerializeField] private GroundDetection groundDetection;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isJumping;
    [SerializeField] private Fireball fireball;
    [SerializeField] private UICharacterController controller;
    [SerializeField] private Energy energy;
    public Energy Energy
    {
        get { return energy; }
    }
    [SerializeField] private Health health;
    public Health Health
    {
        get { return health; }
    }
    [SerializeField] private Transform fireballSpawnPoint;
    [SerializeField] private bool isCooldown;
    [SerializeField] private bool isMovable;
    [SerializeField] private bool isOnRight = true;
    [SerializeField] private int fireballCount = 5;
    [SerializeField] private int hp;
    [SerializeField] private int damage;
    private float damageBonus;
    private float jumpForceBonus;
    private float healthBonus;
    
    public int Damage
    {
        get => damage+(int)damageBonus;
        set => damage = value;
    }
    public BuffReciever buffReciever;
    private Fireball currentFireball;
    private List<Fireball> fireballPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fireballPool = new List<Fireball>();
        for (int i = 0; i < fireballCount; i++)
        {
            var fireballTemp = Instantiate(fireball, fireballSpawnPoint);
            fireballPool.Add(fireballTemp);
            fireballTemp.gameObject.SetActive(false);
        }
        GameManager.Instance.rigidbodyContainer.Add(gameObject, rigitbody);
        buffReciever.OnBuffsChanged += BuffUpdate;
        health.Init(this,hp+(int)healthBonus);
    }

    private void BuffUpdate()
    {
        Buff forceBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Force);
        Buff healthBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Armor);
        Buff damageBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Damage);
        jumpForceBonus = forceBuff == null ? 0 : forceBuff.additiveBonus;
        healthBonus = healthBuff == null ? 0 : healthBuff.additiveBonus;
        damageBonus = damageBuff == null ? 0 : damageBuff.additiveBonus;
        health.SetHealth(hp+(int)healthBonus);
    }

    void FixedUpdate()
    {
        Move();

        animator.SetFloat("Speed", Mathf.Abs(rigitbody.velocity.x));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButton("AttackRange"))
            CheckShoot();        
        if (Input.GetButton("AttackMelee"))
            MeleeAttack();
#endif
    }

    private void Move()
    {
        animator.SetBool("isGrounded", groundDetection.IsGrounded);
        if (!isJumping && !groundDetection.IsGrounded)
            animator.SetTrigger("StartFall");
        isJumping = isJumping && !groundDetection.IsGrounded;
        direction = Vector3.zero;
#if UNITY_EDITOR
        if (isMovable)
        {
            if (Input.GetKey(KeyCode.LeftArrow)  || Input.GetAxis("Horizontal") < 0)
                direction = Vector3.left;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0)
                direction = Vector3.right;
        }
#endif
        if (isMovable)
        {
            if (controller.Left.IsPressed)
                direction = Vector3.left;
            if (controller.Right.IsPressed)
                direction = Vector3.right;
        }

        direction *= speed;
        direction.y = rigitbody.velocity.y;
        if (!isDisabled)
            rigitbody.velocity = direction;
        if (direction.x > 0)
        {
            isOnRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //spriteRenderer.flipX = false;
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

    private void Jump()
    {
        if (groundDetection.IsGrounded)
        {
            rigitbody.AddForce(Vector2.up * (jumpForce+jumpForceBonus), ForceMode2D.Impulse);
            animator.SetTrigger("StartJump");
            isMovable = true;
            isJumping = true;
        }
    }

    public void InitUIController(UICharacterController uiCharacterController)
    {
        controller = uiCharacterController;
        controller.Jump.onClick.AddListener(Jump);
        controller.Fire.onClick.AddListener(CheckShoot);
    }

    #region MELEE ATTACK
    void MeleeAttack()
    {
        if (!isCooldown)
        {
            isMovable = false;
            animator.SetTrigger("StartAttackMelee");
            StartCoroutine(StartCooldown());
        }
    }

    void AttackImpact()
    {
        meleeAttackRegion.SetActive(true);
    }
    
    void AttackFinish()
    {
        isMovable = true;
    }
    #endregion

    #region RANGE ATTACK

    void CheckShoot()
    {
        if (!isCooldown && energy.CheckEnergySufficiency(10))
        {
            isMovable = false;
            animator.SetTrigger("StartAttackRange");
            StartCoroutine(StartCooldown());
        }
    }
    
    void Shoot()
    {
        currentFireball = GetFireballFromPool();
        currentFireball.SetImpulse(isOnRight ? Vector2.right : Vector2.left, this, (int)(damage+damageBonus));
        isMovable = true;
    }

    private IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(attackSpeed);
        isCooldown = false;
    }

    private Fireball GetFireballFromPool()
    {
        if (fireballPool.Count > 0)
        {
            var fireballTemp = fireballPool[0];
            fireballPool.Remove(fireballTemp);
            fireballTemp.gameObject.SetActive(true);
            fireballTemp.transform.parent = null;
            fireballTemp.transform.position = fireballSpawnPoint.transform.position;
            return fireballTemp;
        }

        return Instantiate(fireball, fireballSpawnPoint.position, Quaternion.identity);
    }

    public void ReturnFireballToPool(Fireball fireballTemp)
    {
        if (!fireballPool.Contains(fireballTemp))
            fireballPool.Add(fireballTemp);
        fireballTemp.transform.parent = fireballSpawnPoint;
        fireballTemp.transform.position = fireballSpawnPoint.transform.position;
        fireballTemp.gameObject.SetActive(false);
    }
    #endregion
    
    public void TakeHit()
    {
        IsDisabled = true;
        animator.SetTrigger("TakeDamage");
        StartCoroutine(RecoverTimeout());
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
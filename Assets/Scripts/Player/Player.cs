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

    [Header("COMPONENTS")]  
    [SerializeField] private Rigidbody2D rigitbody;
    [SerializeField] private Animator animator;
    [SerializeField] private UICharacterController uiCharacterController;
    [SerializeField] private SpriteRenderer spriteRenderer;
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
    [SerializeField] private GroundDetection groundDetection;
    [Header("PREFABS")]  
    [SerializeField] private Fireball fireball;
    [Header("VARIABLES")]
    [SerializeField] private int initHP;
    [SerializeField] private int damage;
    [SerializeField] private float jumpForce;
   
    public float Force
    {
        get => jumpForce;
        set => jumpForce = value;
    }
    [SerializeField] private float speed;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    [SerializeField] private float attackSpeed;
    [SerializeField] private float recoveryTime;
    [SerializeField] private float interactionRange;
    [Header("FLAGS")]
    [SerializeField] private bool isCheatMode;
    [SerializeField] private bool isDisabled;
    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isCooldown;
    [SerializeField] private bool isMovable;
    [SerializeField] private bool isOnRight = true;
    [Header("OTHER")] 
    [SerializeField] private GameObject interactableObject;
    [SerializeField] private GameObject meleeAttackRegion;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Transform fireballSpawnPoint;
    [SerializeField] private int fireballCount = 5;
    
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
        health.Init(this,initHP+(int)healthBonus);
    }

    private void BuffUpdate()
    {
        Buff forceBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Force);
        Buff healthBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Armor);
        Buff damageBuff = buffReciever.Buffs.Find(buff => buff.type == BuffType.Damage);
        jumpForceBonus = forceBuff == null ? 0 : forceBuff.additiveBonus;
        healthBonus = healthBuff == null ? 0 : healthBuff.additiveBonus;
        damageBonus = damageBuff == null ? 0 : damageBuff.additiveBonus;
        health.SetHealth(initHP+(int)healthBonus);
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
        if (Input.GetButton("Use"))
            Interact();
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
            if (uiCharacterController.Left.IsPressed)
                direction = Vector3.left;
            if (uiCharacterController.Right.IsPressed)
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
        this.uiCharacterController = uiCharacterController;
        this.uiCharacterController.Jump.onClick.AddListener(Jump);
        this.uiCharacterController.Fire.onClick.AddListener(CheckShoot);
        this.uiCharacterController.Hit.onClick.AddListener(MeleeAttack);
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

    #region INTERACTION

    private void Interact()
    {
        if (interactableObject)
        {
            if (GameManager.Instance.checkpointsContainer.ContainsKey(interactableObject))
            {
                GameManager.Instance.SetCheckpoint(GameManager.Instance.checkpointsContainer[interactableObject]);
            }
        }
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
        StartCoroutine(ReviveTimeout());

    }

    private IEnumerator ReviveTimeout()
    {
        yield return new WaitForSeconds(recoveryTime);
        Revive();
    }
    
    public void Revive()
    {
        if (GameManager.Instance.currentCheckpoint)
        {
            direction = Vector3.zero;
            transform.position = GameManager.Instance.currentCheckpoint.transform.position;
            health.SetFullHealth();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.interactableObjectsContainer.ContainsKey(col.gameObject))
        {
            GameObject obj = GameManager.Instance.interactableObjectsContainer[col.gameObject].gameObject;
            interactableObject = obj;
            uiCharacterController.SetLabel(obj.GetComponent<InteractableObject>().label);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (GameManager.Instance.interactableObjectsContainer.ContainsKey(col.gameObject))
        {
            ResetInteract();
        }
    }

    public void ResetInteract()
    {
        interactableObject = null;
        uiCharacterController.CloseLabel();
    }
    
    #region DEBUG
    /* void OnDrawGizmosSelected()
 {
     if (GameManager.Instance.isDebugMode)
     {
         Gizmos.color = Color.yellow;
         Gizmos.DrawSphere(transform.position, interactionRange);
     }
 }*/
    

    #endregion

}
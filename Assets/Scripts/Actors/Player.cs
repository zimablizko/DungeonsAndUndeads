using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : Actor
{
    #region Singleton

    public static Player Instance { get; private set; }

    #endregion

    [Header("PLAYER COMPONENTS")] 
    private UICharacterController uiCharacterController;

    [SerializeField] private Energy energy;

    public Energy Energy
    {
        get { return energy; }
    }

    [Header("VARIABLES")] 
    [SerializeField] private float reviveTime = 2.5f;
    [SerializeField] private float interactionRange;
    [Header("FLAGS")] 
    [SerializeField] private bool isCheatMode;

    [Header("OTHER")] 
    [SerializeField] private GameObject interactableObject;

    private void Awake()
    {
        Instance = this;
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
        if (Input.GetButtonDown("Use"))
            Interact();
#endif
    }

   


    /*
    private void Move()
    {
        animator.SetBool("isGrounded", groundDetection.IsGrounded);
        if (!isJumping && !groundDetection.IsGrounded)
            animator.SetTrigger("StartFall");
        isJumping = /*isJumping &&#1# !groundDetection.IsGrounded;
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
    }#1#

    private void Jump()
    {
        if (!isCooldown && groundDetection.IsGrounded)
        {
            rigitbody.AddForce(Vector2.up * (jumpForce+jumpForceBonus), ForceMode2D.Impulse);
            animator.SetTrigger("StartJump");
            isMovable = true;
            isJumping = true;
            StartCoroutine(StartCooldown());
        }
    }
    */
    //TODO: у Player checkShoot с учётом энергии

    public void InitUIController(UICharacterController uiCharacterController)
    {
        this.uiCharacterController = uiCharacterController;
        this.uiCharacterController.Jump.onClick.AddListener(Jump);
        this.uiCharacterController.Fire.onClick.AddListener(CheckShoot);
        this.uiCharacterController.Hit.onClick.AddListener(MeleeAttack);
    }


    #region INTERACTION

    private void Interact()
    {
        if (interactableObject)
        {
            if (GameManager.Instance.checkpointsContainer.ContainsKey(interactableObject))
            {
                GameManager.Instance.SetCheckpoint(GameManager.Instance.checkpointsContainer[interactableObject]);
            }

            if (interactableObject.GetComponent<SceneChanger>())
            {
                RoomManager.Instance.ChangeScene();
            }
        }
    }

    #endregion


    private IEnumerator ReviveTimeout()
    {
        yield return new WaitForSeconds(reviveTime);
        Revive();
    }

    public void Revive()
    {
        if (GameManager.Instance.currentCheckpoint)
        {
            Health.SetFullHealth();
            ActorInit();
            transform.position = GameManager.Instance.currentCheckpoint.transform.position;
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
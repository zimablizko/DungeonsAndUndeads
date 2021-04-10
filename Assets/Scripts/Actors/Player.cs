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

    [Header("TTTTTT")] private UICharacterController uiCharacterController;

    [Header("PLAYER COMPONENTS")] [Header("VARIABLES")] [SerializeField]
    private float reviveTime = 2.5f;

    [SerializeField] private float interactionRange;
    [SerializeField] private int dashEnergyCost = 5;


    [Header("INTERACTION")] [SerializeField]
    private GameObject interactableObject;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsDisabled)
        {
            if (Input.GetButtonDown("Jump"))
                Jump();
            if (Input.GetButtonDown("AttackRange"))
                CheckShoot();
            if (Input.GetButtonDown("AttackMelee"))
                MeleeAttack();
            if (Input.GetButtonDown("Use"))
                Interact();
            if (Input.GetButtonDown("Dash"))
                StartDash();
        }

        if (Input.GetButtonDown("Escape"))
            GameObject.Find("Canvas").GetComponent<PauseMenu>().OnClickPause();
        if (Input.GetKey(KeyCode.F3))
        {
            if (GameManager.Instance.isDebugMode)
            {
                Health.SetHealth(9999);
                MeleeDamage = 100;
            }
        }

        if (Input.GetKey(KeyCode.F4))
        {
            if (GameManager.Instance.isDebugMode)
            {
                GFXManager.Instance.ShakeCamera(1f);
            }
        }
    }

    public void InitUIController(UICharacterController uiCharacterController)
        {
            this.uiCharacterController = uiCharacterController;
            this.uiCharacterController.Jump.onClick.AddListener(Jump);
            this.uiCharacterController.Fire.onClick.AddListener(CheckShoot);
            this.uiCharacterController.Hit.onClick.AddListener(MeleeAttack);
        }

        public void TakeIceJail(int hitDamage)
        {
            if (!IsInvulnerable)
                Health.TakeHit(hitDamage);
            rigidbody.velocity = Vector2.zero;
            GFXManager.Instance.CreateFloatingText(transform, hitDamage.ToString());
            IsDisabled = true;
            isMovable = false;
            animator.SetTrigger("TakeIceJail");
        }

        private void RecoverIceJail()
        {
            IsDisabled = false;
            isMovable = true;
        }

        #region INTERACTION

        private void Interact()
        {
            if (interactableObject && isMovable)
            {
                Debug.Log("Interact with " + interactableObject.name);
                if (GameManager.Instance.checkpointsContainer.ContainsKey(interactableObject))
                {
                    GameManager.Instance.SetCheckpoint(GameManager.Instance.checkpointsContainer[interactableObject]);
                }
                else if (interactableObject.GetComponent<SceneChanger>())
                {
                    RoomManager.Instance.ChangeScene();
                }
                else if (interactableObject.GetComponent<ItemComponent>())
                {
                    GameManager.Instance.playerInventory.AddItem(interactableObject.GetComponent<ItemComponent>().Item);
                    AudioManager.Instance.Play("PickupTrinket");
                    GameObject.Destroy(interactableObject);
                }
                else if (interactableObject.GetComponent<RestorationPoint>())
                {
                    interactableObject.GetComponent<RestorationPoint>().Restore();
                }

                ResetInteract();
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

        #endregion

        private void StartDash()
        {
            if (Energy.CurrentEnergy >= dashEnergyCost)
            {
                foreach (GameObject actor in GameManager.Instance.actorsContainer.Keys)
                {
                    if (actor.CompareTag("Enemy"))
                    {
                        actor.GetComponent<Collider2D>().isTrigger = true;
                    }
                }

                Energy.AddEnergy(-dashEnergyCost);
                IsInvulnerable = true;
                isMovable = false;
                animator.SetTrigger("StartDash");
            }
        }

        public void FinishDash()
        {
            foreach (GameObject actor in GameManager.Instance.actorsContainer.Keys)
            {
                if (actor.CompareTag("Enemy"))
                {
                    actor.GetComponent<Collider2D>().isTrigger = false;
                }
            }

            IsInvulnerable = false;
            isMovable = true;
        }

        //Вызывается в последнем фрейме анимации смерти
        public void OnDefeat()
        {
            GameObject.Find("Canvas").GetComponent<PauseMenu>().OnDefeatMenu();
        }

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
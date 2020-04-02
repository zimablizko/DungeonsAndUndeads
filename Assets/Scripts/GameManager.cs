using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   #region Singleton

   public static GameManager Instance { get; private set; }

   #endregion
   
   public Dictionary<GameObject, Health> healthContainer;
   public Dictionary<GameObject, Energy> energyContainer;
   public Dictionary<GameObject, Coin> coinContainer;
   public Dictionary<GameObject, BuffReciever> buffRecieverContainer;
   public Dictionary<GameObject, Rigidbody2D> rigidbodyContainer;
   public Dictionary<GameObject, ItemComponent> itemContainer;
   public Dictionary<GameObject, InteractableObject> interactableObjectsContainer;
   public Dictionary<GameObject, Checkpoint> checkpointsContainer;
   public bool isSoundEnabled;
   public bool isDebugMode;
   public ItemBase itemDataBase;
   public PlayerInventory playerInventory;
   public Player player;
   public Checkpoint currentCheckpoint;
   private void Awake()
   {
      player = FindObjectOfType<Player>();
      if (PlayerPrefs.HasKey("Sound_Enabled"))
         isSoundEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("Sound_Enabled"));
      Instance = this;
      healthContainer = new Dictionary<GameObject, Health>();
      energyContainer = new Dictionary<GameObject, Energy>();
      coinContainer = new Dictionary<GameObject, Coin>();
      buffRecieverContainer = new Dictionary<GameObject, BuffReciever>();
      rigidbodyContainer = new Dictionary<GameObject, Rigidbody2D>();
      itemContainer = new Dictionary<GameObject, ItemComponent>();
      interactableObjectsContainer = new Dictionary<GameObject, InteractableObject>();
      checkpointsContainer = new Dictionary<GameObject, Checkpoint>();
   }

   /**
    * Выключаем старый чекпоинт и добавляем его в интерактивные объекты
    * С новым наоборот
    */
   public void SetCheckpoint(Checkpoint checkpoint)
   {
      if (currentCheckpoint != null)
      {
         interactableObjectsContainer.Add(currentCheckpoint.gameObject,
            currentCheckpoint.GetComponent<InteractableObject>());
         currentCheckpoint.DisableCheckpoint();
      }
      currentCheckpoint = checkpoint;
      currentCheckpoint.EnableCheckpoint();
      interactableObjectsContainer.Remove(currentCheckpoint.gameObject);
      player.ResetInteract();
   }

}

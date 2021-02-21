using System;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    private void Awake()
    {
        RoomManager.Instance.sceneChanger = gameObject;
        GameManager.Instance.CheckSceneChangerActive();
        Debug.Log("SceneChanger Added");
    }

    void Start()
    {
        gameObject.GetComponent<InteractableObject>().label = RoomManager.Instance.gameWorld.GetNextRoom().Name;
    }

}

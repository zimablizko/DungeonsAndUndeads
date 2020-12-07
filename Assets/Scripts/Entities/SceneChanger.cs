using System;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    private void Awake()
    {
        RoomManager.Instance.sceneChanger = gameObject;
    }

    void Start()
    {
        gameObject.GetComponent<InteractableObject>().label = RoomManager.Instance.gameWorld.GetNextRoom().Name;
    }

}

using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<InteractableObject>().label = RoomManager.Instance.gameWorld.GetNextRoom().Name;
    }

}

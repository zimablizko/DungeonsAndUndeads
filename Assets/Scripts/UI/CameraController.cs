using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    private Vector3 offset;


    void Start()
    {
        transform.position =
            new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, 0);
        offset = transform.position - player.transform.position;
    }


    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
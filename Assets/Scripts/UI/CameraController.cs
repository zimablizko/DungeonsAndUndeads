using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float smoothTime = 0.15F;
    private GameObject player;
    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        player = GameManager.Instance.player.gameObject;
        transform.position =
            new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, 0);
    }
    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = player.transform.TransformPoint(new Vector3(offsetX, offsetY, 0));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
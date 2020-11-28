using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRealGrounded;
    [SerializeField] private bool onChecking = false;
    public bool IsGrounded
    {
        get => isGrounded;
        set => isGrounded = value;
    }

    private void Update()
    {
        if (isRealGrounded || onChecking)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.activeSelf && collision.gameObject.CompareTag("Ground"))  
        {
            isRealGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObject.activeSelf && collision.gameObject.CompareTag("Ground"))
        {
            isRealGrounded = false;
            onChecking = true;
            StartCoroutine(CheckDelay());
        }
    }
    
    //Микрозадержка, чтобы можно было прыгнуть с самого края платформы
    private IEnumerator CheckDelay()
    {
        yield return new WaitForSeconds(0.25f);
        if (!isRealGrounded)
        {
            onChecking = false;
        }
        
    }
}

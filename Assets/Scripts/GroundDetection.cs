using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private bool isGrounded;
    
    public bool IsGrounded
    {
        get => isGrounded;
        set => isGrounded = value;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    
    private IEnumerator CheckDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isGrounded = false;
    }
}

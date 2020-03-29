using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool isActive;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isEnabled", isActive);
    }

    public void DisableCheckpoint()
    {
        isActive = false;
    }    
    public void EnableCheckpoint()
    {
        isActive = true;
    }
}

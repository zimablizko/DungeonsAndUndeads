using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorationPoint : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetBool("isEnabled", false);
    }

    public void Restore()
    {
        animator.SetBool("isEnabled", true);
        Player.Instance.Health.SetFullHealth();
        Player.Instance.Energy.SetFullEnergy();
    }    

}

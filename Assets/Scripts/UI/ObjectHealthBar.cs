using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealthBar : MonoBehaviour
{
    private Health health;

    private Vector3 localScale;
    private float initScaleX;
    
    // Start is called before the first frame update
    void Start()
    {
        health = transform.parent.gameObject.GetComponent<Health>();
        localScale = transform.localScale;
        initScaleX = localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
       
        localScale.x = (float)health.CurrentHealth / (float)health.MaxHealth * initScaleX;
        
        transform.localScale = localScale;
    }
}

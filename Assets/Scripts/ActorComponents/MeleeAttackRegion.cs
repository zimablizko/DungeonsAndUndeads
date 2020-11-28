using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackRegion : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private TriggerDamage triggerDamage;
    //[SerializeField] private int damage;
    [SerializeField] private BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        //triggerDamage.Init(this,damage);
    }

    private void OnEnable()
    {

        triggerDamage.Init(this,triggerDamage.Damage);
        StartCoroutine(StartLife());
    }

    private IEnumerator StartLife()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
    
    public void Destroy(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}

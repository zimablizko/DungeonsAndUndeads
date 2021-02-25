using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float speed = 1f;
    void Start()
    {
        StartCoroutine(StartLife());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 1 * speed * Time.deltaTime, 0);
    }
    
    private IEnumerator StartLife()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}

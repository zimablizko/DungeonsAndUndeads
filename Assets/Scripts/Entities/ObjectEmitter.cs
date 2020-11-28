using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEmitter : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float timeInterval;
    void Start()
    {
        StartCoroutine(emitObject());
    }

    private IEnumerator emitObject()
    {
        yield return new WaitForSeconds(timeInterval);
        Instantiate(objectPrefab, transform.position, Quaternion.identity);
        StartCoroutine(emitObject());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

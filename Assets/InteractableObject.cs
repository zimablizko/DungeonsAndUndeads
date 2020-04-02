using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Компонент для описания объекта, с которым можно взаимодействовать
 */
public class InteractableObject : MonoBehaviour
{
    public string label;
    void Start()
    {
        GameManager.Instance.interactableObjectsContainer.Add(gameObject,this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GFXManager : MonoBehaviour
{
    public static GFXManager Instance { get; private set; }

    [SerializeField] private FloatingText floatingTextPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateFloatingText(Transform transform, string text)
    {
        FloatingText floatText = GameObject.Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        floatText.GetComponent<TextMeshPro>().text = text;
    }
}

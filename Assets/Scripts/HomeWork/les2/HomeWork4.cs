using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWork4 : MonoBehaviour
{
    public SpriteRenderer[] renderers;

    void Start()
    {
        foreach (SpriteRenderer renderer in renderers)
            renderer.color = Random.ColorHSV();
    }

}

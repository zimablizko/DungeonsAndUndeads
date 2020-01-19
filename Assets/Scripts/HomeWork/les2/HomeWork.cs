using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWork : MonoBehaviour
{
    int sqrA = 5;
    int rectA = 3;
    int rectB = 7;
    float circleRadius = 6;

    void Start()
    {
        Debug.Log("Площадь квадрата = " + Mathf.Pow(sqrA, 2));
        Debug.Log("Площадь прямоугольника = " + rectA * rectB);
        Debug.Log("Площадь окружности = " + Mathf.Pow(circleRadius, 2) * Mathf.PI);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWork3 : MonoBehaviour
{
    long paperThickness = 1;
    long distance = 300000;

    void Start()
    {
        int i = 0;
        distance *= (long)Mathf.Pow(10, 6);
        while (paperThickness < distance)
        {
            i++;
            paperThickness *= 2;
        }
        Debug.Log(i + " складываний.");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWork2 : MonoBehaviour
{
    byte apples = 3;
    byte oranges = 5;
    byte tomatoes = 2;

    void Start()
    {
        if (apples>oranges && apples>tomatoes)
            if(oranges>tomatoes)
                Debug.Log("яблоки, апельсины, помидоры");
            else
                Debug.Log("яблоки, помидоры, апельсины");
        else if (oranges > apples && oranges > tomatoes)
            if (apples > tomatoes)
                Debug.Log("апельсины, яблоки, помидоры");
            else
                Debug.Log("апельсины, помидоры, яблоки");
        else if (tomatoes > apples && tomatoes > oranges)
            if (apples > oranges)
                Debug.Log("помидоры, яблоки, апельсины");
            else
                Debug.Log("помидоры, апельсины, яблоки");
    }

}

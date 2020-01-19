using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeWork5 : MonoBehaviour
{

    void Start()
    {
        Debug.Log(CheckNumbers(4, 8));
        PrintMessage();
        Debug.Log(GetCircleSqr(3f));
    }

    bool CheckNumbers(int a, int b)
    {
        if (a > b)
            return true;
        else
            return false;
    }

    void PrintMessage()
    {
        Debug.Log("Любое сообщение");
    }

    float GetCircleSqr(float r)
    {
        return Mathf.PI * Mathf.Pow(r, 2);
    }
}

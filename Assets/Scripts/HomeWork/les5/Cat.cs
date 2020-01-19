using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat
{
    private string name;
    private int age;
    private int height;
    private float tailLength;
    public int Weight { get; set; }
    public int Age => age;
    
    public void Meow()
    {
        Debug.Log("Meow");
        Debug.Log("Age = "+Age);
        Debug.Log("Weight = "+Weight);
    }

    public Cat()
    {
        name = "Barsik";
        age = 5;
        Weight = 10;
        height = 3;
        tailLength = 2f;
    }

    public Cat(string name, int age, int weight, int height, float tailLength)
    {
        this.name = name;
        this.age = age;
        Weight = weight;
        this.height = height;
        this.tailLength = tailLength;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HWLes5 : MonoBehaviour
{
    void Start()
    {
        Cat cat = new Cat();
        cat.Meow();
        
        Vehicle car = new Car();
        car.Beep();
        Bus bus = new Bus();
        bus.BeepBeep();
    }
    
}

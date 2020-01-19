using UnityEngine;

public abstract class Vehicle
{
    public string name;
    public abstract void Beep();

    public virtual void BeepBeep()
    {
        Debug.Log("BEEP BEEP");
    }
}

public class Bus : Vehicle
{
    public override void Beep()
    {
        Debug.Log("Big BEEP");
    }

    public override void BeepBeep()
    {
        base.BeepBeep();
    }
}

public class Car : Vehicle
{
    public override void Beep()
    {
        Debug.Log("Small BEEP");
    }
    
    public override void BeepBeep()
    {
        base.BeepBeep();
    }
}

public class Tractor : Vehicle
{
    public override void Beep()
    {
        Debug.Log("Middle BEEP");
    }
    
    public override void BeepBeep()
    {
        base.BeepBeep();
    }
}
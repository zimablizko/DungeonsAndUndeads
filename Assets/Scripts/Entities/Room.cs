using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private string name;

    public string Name => name;

    public Room(string name)
    {
        this.name = name;
    }    
    
    public Room(string[] nameList)
    {
        int index = Random.Range(0, nameList.Length);
        this.name = nameList[index];
    }
}

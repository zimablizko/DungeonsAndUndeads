using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Buff> buffs;

    public List<Buff> Buffs
    {
        get => buffs;
        set => buffs = value;
    }

    public Action OnBuffsChanged;

    private void Start()
    {
        GameManager.Instance.buffReceiverContainer.Add(gameObject,this);
        buffs = new List<Buff>();
    }

    public void AddBuff(Buff buff)
    {
        if (!buffs.Contains(buff))
        {
            buffs.Add(buff);
        }
        OnBuffsChanged?.Invoke();
    }    
    
    public void RemoveBuff(Buff buff)
    {
        if (buffs.Contains(buff))
        {
            buffs.Remove(buff);
        }
        OnBuffsChanged?.Invoke();
    }
}

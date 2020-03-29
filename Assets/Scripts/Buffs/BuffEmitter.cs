using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEmitter : MonoBehaviour
{
    [SerializeField] private Buff buff;
    
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.buffRecieverContainer.ContainsKey(other.gameObject))
        {
            var reciever = GameManager.Instance.buffRecieverContainer[other.gameObject];
            reciever.AddBuff(buff);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GameManager.Instance.buffRecieverContainer.ContainsKey(other.gameObject))
        {
            var reciever = GameManager.Instance.buffRecieverContainer[other.gameObject];
            reciever.RemoveBuff(buff);
        }
    }
}
[Serializable]
public class Buff
{
    public BuffType type;
    public float additiveBonus;
    public float multipleBonus;
}

public enum BuffType
{
    Damage, Force, Armor
}
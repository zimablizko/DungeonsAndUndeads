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
        if (GameManager.Instance.buffReceiverContainer.ContainsKey(other.gameObject))
        {
            var reciever = GameManager.Instance.buffReceiverContainer[other.gameObject];
            reciever.AddBuff(buff);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GameManager.Instance.buffReceiverContainer.ContainsKey(other.gameObject))
        {
            var reciever = GameManager.Instance.buffReceiverContainer[other.gameObject];
            reciever.RemoveBuff(buff);
        }
    }
}

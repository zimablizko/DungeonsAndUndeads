using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int coinsCount;
    [SerializeField] private Text coinsText;
    public BuffReceiver buffReceiver;
    private List<Item> items;

    public List<Item> Items
    {
        get => items;
        set => items = value;
    }

    public int CoinsCount
    {
        get => coinsCount;
        set => coinsCount = value;
    }

    public Action OnItemlistChanged;
    public static PlayerInventory Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.playerInventory = this;
        coinsText.text = coinsCount.ToString();
        items = new List<Item>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.coinContainer.ContainsKey(col.gameObject))
        {
            coinsCount++;
            coinsText.text = coinsCount.ToString();
            var coin = GameManager.Instance.coinContainer[col.gameObject];
            coin.StartDestroy();
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        foreach (var buff in item.BuffList)
        {
            buffReceiver.AddBuff(buff);
        }
        OnItemlistChanged?.Invoke();
    }
    
}

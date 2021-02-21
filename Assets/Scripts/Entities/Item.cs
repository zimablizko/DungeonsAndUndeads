using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    TRINKET = 1,QUEST = 2
}

public enum ItemRarity
{
    COMMON, RARE, EPIC, LEGENDARY
}

[Serializable]
public class Item
{
    [SerializeField] private int id;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private List<Buff> buffList;
    [SerializeField] private ItemType itemType;
    [SerializeField] private ItemRarity itemRarity;
    //[SerializeField] private BuffType buffType;
    //[SerializeField] private float value;
    [SerializeField] private Sprite sprite;
    [SerializeField] private bool isExcluded;

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public List<Buff> BuffList
    {
        get => buffList;
        set => buffList = value;
    }

    public ItemType ItemType
    {
        get => itemType;
        set => itemType = value;
    }

    public ItemRarity ItemRarity
    {
        get => itemRarity;
        set => itemRarity = value;
    }

    public bool IsExcluded
    {
        get => isExcluded;
        set => isExcluded = value;
    }
}
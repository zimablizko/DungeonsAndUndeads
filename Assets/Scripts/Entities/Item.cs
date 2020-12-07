﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    TRINKET = 1,QUEST = 2
}

[Serializable]
public class Item
{

    [SerializeField] private int id;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private List<Buff> buffList;
    [SerializeField] private ItemType itemType;
    //[SerializeField] private BuffType buffType;
    //[SerializeField] private float value;
    [SerializeField] private Sprite sprite;

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
}
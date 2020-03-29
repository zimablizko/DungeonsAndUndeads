using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database",menuName = "Database/Items")]
public class ItemBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField, HideInInspector] private List<Item> items;
    [SerializeField] private Item currentItem;
    private int currentIndex;

    public void CreateItem()
    {
        if(items == null)
            items = new List<Item>();
        Item item = new Item();
        items.Add(item);
        currentItem = item;
        currentIndex = items.Count - 1;
    }

    public void DeleteItem()
    {
        if(items==null || currentItem==null)
            return;
        items.Remove(currentItem);
        if (items.Count > 0)
            currentItem = items[0];
        else CreateItem();
        currentIndex = 0;
    }

    public void NextItem()
    {
        if (currentIndex + 1 < items.Count)
        {
            currentIndex++;
            currentItem = items[currentIndex];
        }
    }    
    
    public void PrevItem()
    {
        if (currentIndex >0)
        {
            currentIndex--;
            currentItem = items[currentIndex];
        }
    }

    public Item GetItemOfID(int id)
    {
        return items.Find(item => item.Id == id);
    }
}

[Serializable]
public class Item
{
    [SerializeField] private int id;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private BuffType buffType;
    [SerializeField] private float value;
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

    public BuffType BuffType
    {
        get => buffType;
        set => buffType = value;
    }

    public float Value
    {
        get => value;
        set => this.value = value;
    }
}
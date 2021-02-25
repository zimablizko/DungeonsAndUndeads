using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Database/Items")]
public class ItemDataBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField, HideInInspector] private List<Item> items;
    [SerializeField] private Item currentItem;
    private int currentIndex;

    public void CreateItem()
    {
        if (items == null)
            items = new List<Item>();
        Item item = new Item();
        items.Add(item);
        currentItem = item;
        currentIndex = items.Count - 1;
    }

    public void DeleteItem()
    {
        if (items == null || currentItem == null)
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
        if (currentIndex > 0)
        {
            currentIndex--;
            currentItem = items[currentIndex];
        }
    }

    public void InitDatabase()
    {
        foreach (var item in items)
        {
            item.IsExcluded = false;
        }
    }
    
    public Item GetItemOfID(int id)
    {
        return items.FindAll(item => item.ItemType == ItemType.TRINKET).Find(item => item.Id == id);
    }

    public Item GetRandomItem()
    {
        List<Item> availableItems = items.FindAll(item => item.ItemType == ItemType.TRINKET && !item.IsExcluded);
        var itemNumber = Random.Range(0, availableItems.Count);
        Item randomItem = availableItems[itemNumber];
        randomItem.IsExcluded = true;
        return randomItem;
    }
}
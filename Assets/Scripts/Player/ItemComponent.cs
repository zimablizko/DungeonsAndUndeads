using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private int itemId;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Item item;

    public Item Item
    {
        get => item;
        set => item = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (itemId > 0)
            UpdateItemById(itemId);
    }

    public void UpdateItemById(int itemId)
    {
        item = GameManager.Instance.itemDataBase.GetItemOfID(itemId);
        spriteRenderer.sprite = item.Sprite;
        //GameManager.Instance.itemContainer.Add(gameObject, this);
        gameObject.GetComponent<InteractableObject>().label = item.ItemName + "\n" + item.Description;
    }

    public void UpdateItem(Item itemObj)
    {
        item = itemObj;
        itemId = item.Id;
        spriteRenderer.sprite = item.Sprite;
        //GameManager.Instance.itemContainer.Add(gameObject, this);
        gameObject.GetComponent<InteractableObject>().label = item.ItemName + "\n" + item.Description;
    }

    public void Destroy(GameObject gameObject)
    {
        MonoBehaviour.Destroy(gameObject);
    }
}
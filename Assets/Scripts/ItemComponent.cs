using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private ItemType itemType;
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
        item = GameManager.Instance.itemDataBase.GetItemOfID((int) itemType);
        spriteRenderer.sprite = item.Sprite;
        GameManager.Instance.itemContainer.Add(gameObject, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum ItemType
    {
        ForcePotion = 1,DefencePotion = 2, DamagePotion = 3
    }

    public void Destroy(GameObject gameObject)
    {
        MonoBehaviour.Destroy(gameObject);
    }
}

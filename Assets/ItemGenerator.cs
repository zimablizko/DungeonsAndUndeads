using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject trinketPosition;
    void Start()
    {
        Item item = GameManager.Instance.itemDataDataBase.GetRandomItem();
        Debug.Log(item.ItemName);
        var trinket = Instantiate(itemPrefab, trinketPosition.transform);
        itemPrefab.GetComponentInChildren<ItemComponent>().UpdateItem(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

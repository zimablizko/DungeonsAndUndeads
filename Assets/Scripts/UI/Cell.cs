using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField]private Image icon;
    private Item item;
    
    public void Init(Item item)
    {
        this.item = item;
        icon.color = new Color(1,1,1,1);
        icon.sprite = item.Sprite;
    }
    
    public void ClearCell()
    {
        icon.color = new Color(1,1,1,0);
        item = null;
        icon.sprite = null;
    }
    void Awake()
    {
        icon.sprite = null;
    }

    public void OnClickCell()
    {
        /*if (item == null)
            return;
        GameManager.Instance.playerInventory.Items.Remove(item);
        Buff buff = new Buff()
        {
            type = item.BuffType,
            additiveBonus = item.Value
        };
        GameManager.Instance.playerInventory.buffReciever.AddBuff(buff);*/
        //TODO: ???
    }
}

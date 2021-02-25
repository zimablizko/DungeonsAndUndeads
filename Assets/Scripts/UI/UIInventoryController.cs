using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    [SerializeField] private Cell[] cells;
    [SerializeField] private int cellCount;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform rootParent;

    public void Init()
    {

        cells = new Cell[cellCount];
        for (int i = 0; i < cellCount; i++)
        {
            cells[i] = Instantiate(cellPrefab, rootParent);
            cells[i].ClearCell();
        }
        GameManager.Instance.playerInventory.OnItemlistChanged += InventoryRefresh;
        //cellPrefab.gameObject.SetActive(false);
    }

    /*private void OnEnable()
    {
        if (cells == null || cells.Length <= 0)
            Init();
        InventoryRefresh();
    }*/

    private void InventoryRefresh()
    {
        var inventory =  GameManager.Instance.playerInventory;
        for (int i = 0; i < cellCount; i++)
        {
            cells[i].ClearCell();
        }
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if (i < cells.Length)
                cells[i].Init(inventory.Items[i]);
        }
    }
}

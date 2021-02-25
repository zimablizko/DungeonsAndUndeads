using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDataBase))]
public class ItemBaseEditor : Editor
{
    private ItemDataBase itemDataBase;

    private void Awake()
    {
        itemDataBase = (ItemDataBase) target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New Item"))
        {
            itemDataBase.CreateItem();
        }        
        if (GUILayout.Button("Remove Item"))
        {
            itemDataBase.DeleteItem();
        }  
        if (GUILayout.Button("<="))
        {
            itemDataBase.PrevItem();
        }
        if (GUILayout.Button("=>"))
        {
            itemDataBase.NextItem();
        }        

        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}
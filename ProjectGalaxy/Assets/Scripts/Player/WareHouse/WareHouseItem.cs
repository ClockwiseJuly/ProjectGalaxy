using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WareHouseItem : MonoBehaviour
{
    public GameData gameData;
    public Sprite icon;
    public string name;
    public string description;
    public bool hasItem = false;

    private void OnEnable()
    {
        int myIndex = transform.GetSiblingIndex();
        if (gameData.warehouse.Count > myIndex)
        {
            hasItem = true;
            name = gameData.warehouse[myIndex].name;
            description = gameData.warehouse[myIndex].description;
            icon = gameData.warehouse[myIndex].icon;
            
        }
        
    }

    private void OnDisable()
    {
        if (hasItem)
        {
            hasItem = false;
            name = null;
            description = null;
            icon = null;


        }
    }
}

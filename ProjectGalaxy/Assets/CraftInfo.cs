using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftInfo : MonoBehaviour
{
    public string craftName;
    public string craftInfo;
    public int craftID;
    public List<requiredItem> requiredItems; 
    
    
    [System.Serializable]
    public class requiredItem
    {
        public string itemName;
        public int itemAmount;
        public bool required = false;
    }
}

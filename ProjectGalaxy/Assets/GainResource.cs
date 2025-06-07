using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainResource : MonoBehaviour
{
    public enum ResourceType
    {
        magnify,
        cloud,
        house,
        light,
        eye,
        skull
        
    }
    
    public ResourceType resourceType;
    
    public bool collected = false;

    private void OnDisable()
    {
        if (collected)
        {
            if (resourceType == ResourceType.magnify)
            {
                InventoryManager.Instance.GainMagnifyResource();
            }
            else if (resourceType == ResourceType.cloud)
            {
                InventoryManager.Instance.GainCloudResource();
            }
            else if (resourceType == ResourceType.house)
            {
                InventoryManager.Instance.GainHouseResource();
            }
            else if(resourceType == ResourceType.light)
            {
                InventoryManager.Instance.GainLightResource();
            }
            else if (resourceType == ResourceType.eye)
            {
                InventoryManager.Instance.GainEyeResource();
            }
            else if (resourceType == ResourceType.skull)
            {
                InventoryManager.Instance.GainSkullResource();
            }
        }   
    }
}

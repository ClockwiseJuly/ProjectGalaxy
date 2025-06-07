using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 1)]
public class GameData : ScriptableObject
{
    
    
    [System.Serializable]
    public class Item
    {
        public Sprite icon; // 物品图标
        public string name; // 物品名称
        public string description; // 物品描述
        public int amount; // 物品数量
    }
    
    [SerializeField]
    public List<Item> warehouse = new List<Item>(); // 背包物品列表
    
    [SerializeField]
    public bool playedIntro = false;
    public bool osPaneltutorialtaught = false;
    
    
}

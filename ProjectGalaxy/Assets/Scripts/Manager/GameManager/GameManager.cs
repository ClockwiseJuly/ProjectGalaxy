using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    [FormerlySerializedAs("onTraverse")] public bool playerOnWormHole = false; //玩家是否在虫洞
    public GameObject planetBackground;
    public GameObject wormholeBackground;
    public GameObject planet;
    
    protected override void Start()
    {
        base.Start();

        
        //订阅事件
        GameEvent.OnTraverse += HandleOnTraverse; 
        GameEvent.OnTraverseCompleted += HandleOnTraverseCompleted; 
    }

    
    //跃迁中调用
    private void HandleOnTraverse()
    {
        Debug.Log("GameManager : 跃迁");
        HandleOnWormhole(true);

        UIManager.Instance.TogglePause();
    }

    //跃迁完成调用
    private void HandleOnTraverseCompleted()
    {
        Debug.Log("GameManager : 跃迁完成");
        HandleOnWormhole(false);
        
        
        UIManager.Instance.TogglePause();
        
        
    }

    private void HandleOnWormhole(bool _isOnWormhole)
    {
        playerOnWormHole = _isOnWormhole;
        
        if (playerOnWormHole)
        {
            planetBackground.SetActive(false);
            planet.SetActive(false);
            wormholeBackground.SetActive(true);
        }
        else
        {
            planetBackground.SetActive(true);
            planet.SetActive(true);
            wormholeBackground.SetActive(false);
        }
        

    }
    
    
    
    
}

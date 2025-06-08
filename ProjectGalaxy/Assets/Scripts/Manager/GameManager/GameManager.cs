using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;
using DG.Tweening;
using TMPro;


public class GameManager : Singleton<GameManager>//,IPointerClickHandler
{
    public bool playerOnWormHole = false; //玩家是否在虫洞
    public GameObject planetBackground;
    public GameObject wormholeBackground;
    public GameObject planet;
    public Image nowPlanetImage;
    public Canvas uiCanvas;
    public Image wormholeImage;
    public Image wormholeImage2;
    public Image wormholeImageOrigin;
    
    [Header("===== 选择星球 =====")]
    public GameObject selectedPlanetPanel;
    
    [Header("===== Fungus =====")]
    public Flowchart flowchart;
    
    [Header("===== 收集资源 =====")]
    public CollectResources collectResources;
    public RandomEvent randomEvent;
    
    [Header("===== 游戏轮次管理 =====")]
    private int currentRound = 1;        // 当前轮次
    private int maxRounds = 7;           // 最大轮次
    

    protected override void Start()
    {
        base.Start();
        wormholeImageOrigin = wormholeImage;
        
        //订阅事件
        GameEvent.OnTraverse += HandleOnTraverse; 
        GameEvent.OnTraverseCompleted += HandleOnTraverseCompleted; 
        GameEvent.OnFinishSelectingPlanet += HandleOnFinishSelectingPlanet; 
    }

    private void Update()
    {
        if (wormholeBackground.activeInHierarchy)
        {
            playerOnWormHole = true;
        }
        else
        {
            playerOnWormHole = false;
        }
        
        
    }


    //跃迁中调用
    private void HandleOnTraverse()
    {
        Debug.Log("GameManager : 跃迁");
        
        
        HandleOnWormhole(true);

        UIManager.Instance.TogglePause(true);
    }

    //跃迁完成调用
    private void HandleOnTraverseCompleted()
    {
        Debug.Log("GameManager : 跃迁完成");
        HandleOnWormhole(false);
        
        // 轮次递增逻辑
        currentRound++;
        Debug.Log($"当前轮次：{currentRound}");
        
        // 检查是否达到最大轮次
        if (currentRound > maxRounds)
        {
            // 游戏结束，进入结束场景
            HandleGameEnd();
            return;
        }
        
        // 刷新星球池
        RefreshStarPool();
        
        //UIManager.Instance.TogglePause(false);
    }
    
    // 新增：刷新星球池方法
    private void RefreshStarPool()
    {
        if (StarPoolManager.Instance != null)
        {
            // 通知StarPoolManager当前轮次
            StarPoolManager.Instance.SetCurrentRound(currentRound);
            
            // 获取新的星球信息
            StarInfo[] newStarInfos = StarPoolManager.Instance.GetRandomStarInfosForRound();
            
            // 通知RollingUI刷新
            RollingUI rollingUI = FindObjectOfType<RollingUI>();
            if (rollingUI != null)
            {
                rollingUI.RefreshStarOptions(newStarInfos);
            }
        }
    }
    
    // 新增：游戏结束处理
    private void HandleGameEnd()
    {
        Debug.Log("游戏结束！进入结束场景");
        
        // 触发游戏结束事件
        //OnGameEnd?.Invoke();
        
        // 这里可以添加进入结束场景的逻辑
        // 例如：SceneManager.LoadScene("EndScene");
        // 或者显示结束UI等
        
        // 临时实现：显示结束信息
        PopupText("游戏结束！恭喜完成所有轮次！", 0);
    }
    
    // 新增：获取当前轮次的公共方法
    public int GetCurrentRound()
    {
        return currentRound;
    }
    
    // 新增：重置游戏轮次（用于重新开始游戏）
    public void ResetGameRounds()
    {
        currentRound = 1;
        Debug.Log("游戏轮次已重置");
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

    private void HandleOnFinishSelectingPlanet(Image img)
    {
        Debug.Log("GameManager: 已选择星球");

        wormholeImage = wormholeImage2;
        EffectManager.Instance.CallTraverseCompleted();
        
        nowPlanetImage.sprite = img.sprite;
        collectResources.nowPlanet.sprite = img.sprite;
        

    }

    public void BackToOriginWormholeImg()
    {
        wormholeImage = wormholeImageOrigin;
    }

    public void SeletcPlanet()
    {
        
    }
    
    
    
    public void DialogueOSPanelTutorial()
    {
        flowchart.ExecuteBlock("T2");
    }
    
    public void DialogueTraverseTutorial()
    {
        flowchart.ExecuteBlock("T3");
    }

    public void StartCollect()
    {
        if (playerOnWormHole)
        {
            
            PopupText("在时空隧道中无法收集资源",0);
            return;
        }

        collectResources.gameObject.SetActive(true);
        collectResources.StartResourceGame();
    }

    public void CallTraverseCompleted()
    {
        if (!playerOnWormHole)
        {
            PopupText("暂时无法跃迁",1);
            return;
        }
        
        // 先显示界面
        UIManager.Instance.ShowStarSelectCanvas();
        
        // 然后刷新星球池（这样刷新会在界面激活后进行）
        RefreshStarPool();
    }
    
    
    
    [Header("===== 上升文字 =====")]
    public GameObject risingTextPrefab; // 上升文字的预制体
    public float riseHeight = 100f;     
    public float animationDuration = 1f; // 动画持续时间
    public string displayText = "new";   
    public Ease easeType = Ease.OutQuad; // 动画缓动类型 

    public Button collectBtn;  
    public Button traverseBtn; 
    
    private void PopupText(string _text,int _index = 0)
    {
        Debug.Log("上升文字");
        
        // 实例化文本
        GameObject textObj = null;
        if (_index == 0)
        {
            textObj  = Instantiate(risingTextPrefab, collectBtn.transform.position, Quaternion.identity,uiCanvas.transform);
            
        }
        else if(_index == 1)
        {
            textObj = Instantiate(risingTextPrefab, traverseBtn.transform.position, Quaternion.identity,uiCanvas.transform);
        }
        
        TextMeshProUGUI tmpText = textObj.GetComponent<TextMeshProUGUI>();
        tmpText.text = _text;

        //启动上升协程
        StartCoroutine(FloatAndDestroyText(tmpText.rectTransform));

    }
    
    IEnumerator FloatAndDestroyText(RectTransform rectTransform)
    {
        float duration = 1.5f;    // 持续时间
        float speed = 50f;        // 上升速度

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // 每帧向上移动
            rectTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 销毁对象
        Destroy(rectTransform.gameObject);
    }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     Debug.Log("GameManager : OnPointerClick");
    //     
    //     if (eventData.pointerPress != null)
    //     {
    //         if (eventData.pointerPress.gameObject.name == "采集")
    //         {
    //             Debug.Log("点击采集按钮");
    //             
    //             if(!playerOnWormHole)
    //                 StartCollect();
    //             else
    //             {
    //                 
    //             }
    //                 
    //             
    //         }
    //         else if(eventData.pointerPress.gameObject.name == "确认跃迁")
    //         {
    //             Debug.Log("点击跃迁按钮");
    //             
    //             if (playerOnWormHole)
    //             {
    //                 
    //             }
    //                 
    //             
    //         }
    //     }
    // }
}

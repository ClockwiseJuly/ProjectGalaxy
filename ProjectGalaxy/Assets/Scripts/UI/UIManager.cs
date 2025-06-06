using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("===== 角色状态 =====")]
    
    [Header("===== 计时器设置 =====")]
    public float stayTime = 42f; 
    public bool isPaused = false; //是否暂停
    

    [Header("===== 倒计时UI =====")] 
    public GameObject countdownUI;
    public Slider countdownSlider;
    public TextMeshProUGUI countdownText;
    public GameObject confirmTraverseUI;
    
    [Header("===== 动画设置 =====")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float jumpStrength = 0.5f;
    public float jumpDuration = 0.3f;
    
    private float currentTime; // 当前剩余时间
    private int lastDisplayedSecond = -1; // 用于跟踪上次显示的秒数
    
    [Header("===== 背包 =====")] 
    public GameObject inventoryMenu;


    [Header("===== 面板 =====")] 
    public GameObject SettingMenu;
    public GameObject OSPanel;
    public bool isEsc = false; //是否按下Esc键
    
    protected override void Start()
    {
        base.Start();
        
        // 初始化时间
        currentTime = stayTime;
        // 设置Slider范围
        countdownSlider.minValue = 0;
        countdownSlider.maxValue = stayTime;
        
        inventoryMenu.gameObject.SetActive(false);
        
        UpdateUI();
        

    }
    
    void Update()
    {
        UpdateForPressEsc();
        
        if (!isPaused && currentTime > 0)
        {
            //倒计时
            currentTime -= Time.deltaTime;
            
            //更新UI
            UpdateUI();
            
            //倒计时结束
            if (currentTime <= 0)
            {
                currentTime = 0;
                Debug.Log("跃迁倒计时结束！");
                //可以添加结束时的逻辑
            }
        }
    }
    
    // 更新UI显示
    void UpdateUI()
    {
        int seconds = (int)currentTime;
        countdownText.text = seconds.ToString("00") + "s";
        countdownSlider.value  += Time.deltaTime;
        
        // 检查是否需要更新颜色或触发动画
        if (seconds <= 10)
        {
            // 设置警告颜色
            countdownText.color = warningColor;
            
            // 只有当秒数变化时才触发动画
            if (seconds != lastDisplayedSecond)
            {
                // 跳跃动画
                countdownText.transform.DOComplete(); // 完成任何正在进行的动画
                countdownText.transform.DOPunchScale(Vector3.one * jumpStrength, jumpDuration);

            }
        }
        else
        {
            // 恢复正常颜色
            countdownText.color = normalColor;
        }
        
        lastDisplayedSecond = seconds; // 更新最后显示的秒数
    }

    void UpdateUIState()
    {
        if (GameManager.Instance.playerOnWormHole)
        {
            isPaused = true;
            countdownUI.SetActive(false);
        }
        else
        {
            countdownUI.SetActive(true);
        }
    }

    void UpdateForPressEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleEsc();

        }

        if (OSPanel.activeInHierarchy)
        {
            isPaused = true;

        }
        else
        {
            isPaused = false;
        }
        
        Time.timeScale = isPaused ? 0 : 1;
        
        
    }
    
    //切换暂停状态
    public void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log(isPaused ? "计时已暂停" : "计时已开始");
        if (!isPaused)
        {
            countdownUI.SetActive(true);
            currentTime = stayTime;
            
            confirmTraverseUI.SetActive(false);
        }
        else
        {
            
            
            countdownUI.SetActive(false);
            confirmTraverseUI.SetActive(true);
        }
        //Time.timeScale = isPaused ? 0 : 1;
        
    }
    
    public void ToggleEsc()
    {
        isEsc = !isEsc;
        Debug.Log(isEsc ? "已打开设置面板" : "已关闭设置面板");
        if (isEsc)
        {
            SettingMenu.SetActive(true);
        }
        else
        {
            SettingMenu.SetActive(false);
        }
        
        
        Time.timeScale = isEsc ? 0 : 1;
        
    }

    public void CheckIfInWormHole()
    {
        
    }

    public void Pause()
    {
        isPaused = true;
    }
    
    //重置倒计时
    public void ResetTimer()
    {
        currentTime = stayTime;
        isPaused = false;
        countdownText.color = normalColor; // 重置颜色
        UpdateUI();
    }
}

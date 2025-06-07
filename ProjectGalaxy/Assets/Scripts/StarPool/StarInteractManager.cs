using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

// 星球交互场景管理器 - 专注于管理UI和星球设置
public class StarInteractManager : MonoBehaviour
{
    [Header("星球交互设置")]
    [SerializeField] private GameObject canvasStarInteract;
    [SerializeField] private Image starImage;
    [SerializeField] private Animator starAnimator;
    
    [Header("星球信息显示UI")]
    [SerializeField] private GameObject starNamePanel; // 星球名称面板
    [SerializeField] private TextMeshProUGUI starNameText; // 星球名称文本
    
    [SerializeField] private GameObject starInfoPanel; // 星球介绍面板
    [SerializeField] private TextMeshProUGUI starInfoText; // 星球介绍文本
    
    [Header("调试设置")]
    [SerializeField] private bool enableDebugLog = true;
    
    // 星球悬浮事件委托
    public static event Action<string, string> OnStarHover; // 鼠标悬浮时触发（名称，介绍）
    public static event Action OnStarExit; // 鼠标离开时触发
    
    void Start()
    {
        // 只有管理器才订阅事件
        OnStarHover += ShowStarInfo;
        OnStarExit += HideStarInfo;
        
        // 初始化时隐藏信息面板
        HideStarInfo();
        
        if (enableDebugLog)
        {
            Debug.Log("StarInteractManager: 管理器初始化完成，已订阅UI事件");
        }
    }
    
    void OnEnable()
    {
        // 订阅事件
        OnStarHover += ShowStarInfo;
        OnStarExit += HideStarInfo;
        
        if (enableDebugLog)
        {
            Debug.Log("StarInteractManager OnEnable 被调用");
        }
    
        // 只有在有有效星球数据时才设置星球交互对象
        if (StarDataManager.HasValidSelectedStar())
        {
            SetupStarInteract();
            // if (enableDebugLog)
            // {
            //     Debug.Log("已设置可交互星球对象。");
            // }
        }
        else
        {
            if (enableDebugLog)
            {
                //Debug.Log("暂无有效星球数据，跳过星球交互对象设置。");
            }
        }
    }
    
    void OnDestroy()
    {
        // 取消订阅事件
        OnStarHover -= ShowStarInfo;
        OnStarExit -= HideStarInfo;
    }
    
    // 移除 IPointerEnterHandler 和 IPointerExitHandler 接口实现
    // 移除 OnPointerEnter 和 OnPointerExit 方法
    
    #region UI显示控制
    
    /// <summary>
    /// 显示星球信息面板
    /// </summary>
    /// <param name="starName">星球名称</param>
    /// <param name="description">星球描述</param>
    private void ShowStarInfo(string starName, string description)
    {
        if (enableDebugLog)
        {
            Debug.Log($"StarInteractManager: ShowStarInfo被调用 - 名称：{starName}，描述：{description}");
        }
        
        // 显示星球名称面板
        if (starNamePanel != null && starNameText != null)
        {
            if (enableDebugLog)
            {
                Debug.Log("StarInteractManager: 设置星球名称面板");
            }
            starNameText.text = starName;
            starNamePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("StarInteractManager: StarNamePanel或StarNameText为空！请在Inspector中设置引用。");
        }
        
        // 显示星球介绍面板
        if (starInfoPanel != null && starInfoText != null)
        {
            if (enableDebugLog)
            {
                Debug.Log("StarInteractManager: 设置星球描述面板");
            }
            starInfoText.text = description;
            starInfoPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("StarInteractManager: StarInfoPanel或StarInfoText为空！请在Inspector中设置引用。");
        }
    }
    
    /// <summary>
    /// 隐藏星球信息面板
    /// </summary>
    private void HideStarInfo()
    {
        if (enableDebugLog)
        {
            Debug.Log("StarInteractManager: 隐藏星球信息面板");
        }
        
        if (starNamePanel != null)
        {
            starNamePanel.SetActive(false);
        }
        
        if (starInfoPanel != null)
        {
            starInfoPanel.SetActive(false);
        }
    }
    
    #endregion
    
    #region 星球设置
    
    /// <summary>
    /// 设置星球交互对象
    /// </summary>
    void SetupStarInteract()
    {
        if (canvasStarInteract == null)
        {
            Debug.LogError("canvasStarInteract 未设置！请在Inspector中指定Canvas Scene下的starinteract对象。");
            return;
        }

        // 激活starinteract对象
        canvasStarInteract.SetActive(true);
        Debug.Log($"已激活星球对象: {canvasStarInteract.name}");

        // 设置星球贴图到Image组件
        SetupStarImage();
        
        // 设置动画控制器
        SetAnimatorController();
        
        //Debug.Log("已设置可交互星球对象。");
    }
    
    /// <summary>
    /// 设置星球图像
    /// </summary>
    private void SetupStarImage()
    {
        Image targetImage = starImage;
        
        // 如果没有手动指定，尝试自动获取
        if (targetImage == null)
        {
            targetImage = canvasStarInteract.GetComponent<Image>();
        }
        
        if (targetImage != null)
        {
            targetImage.sprite = StarDataManager.SelectedStarSprite;
            //Debug.Log($"已设置星球贴图：{StarDataManager.SelectedStarSprite.name}");
            
            // 确保Raycast Target已启用
            if (!targetImage.raycastTarget)
            {
                Debug.LogWarning("Image组件的Raycast Target未启用，鼠标事件可能无法触发！");
            }
        }
        else
        {
            Debug.LogError("canvasStarInteract 缺少 Image 组件！");
        }
    }
    
    /// <summary>
    /// 设置Animator Controller
    /// </summary>
    private void SetAnimatorController()
    {
        Animator animator = starAnimator;
        
        // 如果没有手动指定，尝试自动获取
        if (animator == null)
        {
            animator = canvasStarInteract.GetComponent<Animator>();
        }
        
        if (animator == null)
        {
            Debug.LogError("canvasStarInteract 缺少 Animator 组件！");
            return;
        }
        
        string folderName = StarDataManager.SelectedStarFolderName;
        if (string.IsNullOrEmpty(folderName))
        {
            Debug.LogError("星球文件夹名称为空，无法加载Animator Controller！");
            return;
        }
        
        // 构建Animator Controller的路径
        string controllerPath = $"Stars/{folderName}/{folderName}";
        
        // 从Resources加载Animator Controller
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(controllerPath);
        
        if (controller != null)
        {
            animator.runtimeAnimatorController = controller;
            Debug.Log($"成功加载并设置Animator Controller：{controllerPath}");
        }
        else
        {
            Debug.LogError($"无法加载Animator Controller：{controllerPath}。请确保文件存在于Resources文件夹中。");
        }
    }
    
    #endregion
    
    #region 场景管理
    
    /// <summary>
    /// 返回星球选择场景
    /// </summary>
    public void ReturnToStarSelect()
    {
        // 清除星球数据
        StarDataManager.ClearSelectedStar();
        
        // 返回选择场景
        SceneManager.LoadScene("StarSelect");
    }
    
    #endregion

    /// <summary>
    /// 触发星球悬停事件的公共方法
    /// </summary>
    public static void TriggerStarHover(string starName, string starDescription)
    {
        OnStarHover?.Invoke(starName, starDescription);
    }

    /// <summary>
    /// 触发星球退出事件的公共方法
    /// </summary>
    public static void TriggerStarExit()
    {
        OnStarExit?.Invoke();
    }
}
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
    // 移除starInteractPrefab和spawnPoint，因为不再需要实例化
    // [SerializeField] private GameObject starInteractPrefab;
    // [SerializeField] private Transform spawnPoint;
    
    [Header("Canvas Scene下的starinteract对象")]
    [SerializeField] private GameObject canvasStarInteract; // Canvas Scene下的starinteract对象
    [SerializeField] private Image starImage; // starinteract的Image组件
    [SerializeField] private Animator starAnimator; // starinteract的Animator组件

    // 移除currentStarInstance，因为不再动态创建
    // private GameObject currentStarInstance;

    void Start()
    {
        // 不在Start中创建星球，等待被调用
    }

    // 新增：当Canvas StarInteract被激活时调用
    void OnEnable()
    {
        Debug.Log("StarInteractManager: OnEnable被调用");
        Debug.Log($"StarInteractManager: GameObject名称: {gameObject.name}");
        Debug.Log($"StarInteractManager: GameObject激活状态: {gameObject.activeInHierarchy}");
        
        // 检查是否有有效的选中星球数据
        if (!StarDataManager.HasValidSelectedStar())
        {
            Debug.LogWarning("没有有效的星球数据！请先选择一个星球。");
            return;
        }

        // 设置星球交互对象
        SetupStarInteract();
        Debug.Log("已设置可交互星球对象。");
    }

    // 新增：当Canvas StarInteract被禁用时调用
    void OnDisable()
    {
        // 清理操作（如果需要的话）
        if (canvasStarInteract != null)
        {
            canvasStarInteract.SetActive(false);
        }
    }
    
    // 修改：设置星球交互对象（替代之前的CreateStarInteract）
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
        if (starImage != null)
        {
            starImage.sprite = StarDataManager.SelectedStarSprite;
            Debug.Log($"已设置星球贴图到Image组件：{StarDataManager.SelectedStarSprite.name}");
        }
        else
        {
            // 如果没有手动指定，尝试自动获取
            Image imageComponent = canvasStarInteract.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = StarDataManager.SelectedStarSprite;
                Debug.Log($"已设置星球贴图到自动获取的Image组件：{StarDataManager.SelectedStarSprite.name}");
            }
            else
            {
                Debug.LogError("canvasStarInteract 缺少 Image 组件！");
            }
        }
    
        // 设置动画控制器
        SetAnimatorController();
    
        Debug.Log("已设置可交互星球对象。");
    }
    
    // 修改：设置Animator Controller
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
    
    // 返回星球选择场景
    public void ReturnToStarSelect()
    {
        // 清除星球数据
        StarDataManager.ClearSelectedStar();
        
        // 返回选择场景
        SceneManager.LoadScene("StarSelect");
    }
    
    void OnDestroy()
    {
        // 场景销毁时清除数据（可选）
        // StarDataManager.ClearSelectedStar();
    }
}
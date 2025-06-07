using UnityEngine;
using UnityEngine.SceneManagement;

// 星球交互场景管理器
public class StarInteractManager : MonoBehaviour
{
    [Header("星球交互设置")]
    [SerializeField] private GameObject starInteractPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentStarInstance;

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

        // 生成星球交互对象
        CreateStarInteract();
        Debug.Log("已生成可交互星球对象。");
    }

    // 新增：当Canvas StarInteract被禁用时调用
    void OnDisable()
    {
        // 清理当前星球实例
        if (currentStarInstance != null)
        {
            Destroy(currentStarInstance);
            currentStarInstance = null;
        }
    }
    
    // 创建星球交互对象
    void CreateStarInteract()
    {
        if (starInteractPrefab == null)
        {
            Debug.LogError("starInteractPrefab 未设置！");
            return;
        }
    
        // 确定生成位置
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;
    
        // 实例化星球预制体
        currentStarInstance = Instantiate(starInteractPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"已生成星球对象: {currentStarInstance.name} 在位置: {currentStarInstance.transform.position}"); // 添加这行调试

        // 设置星球贴图
        SpriteRenderer spriteRenderer = currentStarInstance.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = StarDataManager.SelectedStarSprite;
            Debug.Log($"已设置星球贴图：{StarDataManager.SelectedStarSprite.name}");
        }
    
        // 设置动画控制器
        SetAnimatorController();
    
        currentStarInstance.name = $"InteractiveStar_{StarDataManager.SelectedStarIndex}";
        Debug.Log("已生成可交互星球对象。");
    }
    
    // 设置Animator Controller
    private void SetAnimatorController()
    {
        if (currentStarInstance == null)
        {
            Debug.LogError("currentStarInstance 为空，无法设置Animator Controller！");
            return;
        }
        
        Animator animator = currentStarInstance.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("starInteractPrefab 缺少 Animator 组件！");
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
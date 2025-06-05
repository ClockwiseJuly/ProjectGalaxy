using UnityEngine;
using UnityEngine.SceneManagement;

// 星球交互场景管理器
public class StarInteractManager : MonoBehaviour
{
    [Header("星球交互设置")]
    [SerializeField] private GameObject starInteractPrefab;  // starinteract预制体
    [SerializeField] private Transform spawnPoint;           // 生成位置（可选，默认为屏幕中央）
    
    private GameObject currentStarInstance;                  // 当前生成的星球实例
    
    void Start()
    {
        // 检查是否有有效的选中星球数据
        if (!StarDataManager.HasValidSelectedStar())
        {
            Debug.LogError("没有有效的星球数据！返回选择场景。");
            SceneManager.LoadScene("StarSelect");
            return;
        }
        
        // 生成星球交互对象
        CreateStarInteract();
    }
    
    // 创建星球交互对象
    private void CreateStarInteract()
    {
        if (starInteractPrefab == null)
        {
            Debug.LogError("starInteractPrefab 未设置！");
            return;
        }
        
        // 确定生成位置（如果没有指定spawnPoint，则使用屏幕中央）
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        
        // 生成星球对象
        currentStarInstance = Instantiate(starInteractPrefab, spawnPosition, Quaternion.identity);
        
        // 设置星球贴图
        SpriteRenderer spriteRenderer = currentStarInstance.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = StarDataManager.SelectedStarSprite;
            Debug.Log($"成功设置星球贴图：{StarDataManager.SelectedStarSprite.name}");
        }
        else
        {
            Debug.LogError("starInteractPrefab 缺少 SpriteRenderer 组件！");
        }
        
        // 设置Animator Controller
        SetAnimatorController();
        
        // 设置星球名称
        currentStarInstance.name = $"StarInteract_{StarDataManager.SelectedStarFolderName}";
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
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 星球交互触发器 - 只负责检测鼠标事件并触发静态事件
/// </summary>
public class StarInteractTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("调试信息")]
    [SerializeField] private bool enableDebugLog = true;
    
    /// <summary>
    /// 鼠标进入时触发
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (enableDebugLog)
        {
            Debug.Log($"=== StarInteractTrigger: 鼠标进入 {gameObject.name} ===");
            Debug.Log($"鼠标位置: {eventData.position}");
            Debug.Log($"星球名称：{StarDataManager.SelectedStarName}");
        }
        
        // 验证数据有效性
        if (string.IsNullOrEmpty(StarDataManager.SelectedStarName) || 
            string.IsNullOrEmpty(StarDataManager.SelectedStarDescription))
        {
            Debug.LogWarning("StarInteractTrigger: 星球数据为空，无法触发悬停事件");
            return;
        }
        
        // 通过管理器的公共方法触发事件
        StarInteractManager.TriggerStarHover(
            StarDataManager.SelectedStarName, 
            StarDataManager.SelectedStarDescription
        );
    }
    
    /// <summary>
    /// 鼠标离开时触发
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (enableDebugLog)
        {
            Debug.Log($"=== StarInteractTrigger: 鼠标离开 {gameObject.name} ===");
        }
        
        // 通过管理器的公共方法触发事件
        StarInteractManager.TriggerStarExit();
    }
    
    void Start()
    {
        // 检查必要组件
        var image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError($"StarInteractTrigger: {gameObject.name} 缺少 Image 组件！");
        }
        else
        {
            Debug.Log($"StarInteractTrigger: Image 组件存在");
            Debug.Log($"StarInteractTrigger: Raycast Target = {image.raycastTarget}");
            Debug.Log($"StarInteractTrigger: Alpha = {image.color.a}");
            
            if (!image.raycastTarget)
            {
                Debug.LogWarning($"StarInteractTrigger: {gameObject.name} 的 Image 组件 Raycast Target 未启用！");
            }
        }
        
        // 检查 Canvas 和 GraphicRaycaster
        var canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError($"StarInteractTrigger: {gameObject.name} 没有找到父级 Canvas！");
        }
        else
        {
            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster == null)
            {
                Debug.LogError($"StarInteractTrigger: Canvas 缺少 GraphicRaycaster 组件！");
            }
            else
            {
                Debug.Log($"StarInteractTrigger: GraphicRaycaster 存在且启用 = {raycaster.enabled}");
            }
        }
        
        // 检查 EventSystem
        var eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogError("StarInteractTrigger: 场景中没有找到 EventSystem！");
        }
        else
        {
            Debug.Log($"StarInteractTrigger: EventSystem 存在且启用 = {eventSystem.enabled}");
        }
        
        if (enableDebugLog)
        {
            Debug.Log($"StarInteractTrigger: {gameObject.name} 初始化完成");
        }
    }
    
}
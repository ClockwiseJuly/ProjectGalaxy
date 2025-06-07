using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OptionClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // 检查当前选项是否是中间的可交互选项
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null && canvasGroup.blocksRaycasts)
        {
            Debug.Log("进入" + GetComponentInChildren<Text>().text);
            
            // 获取当前选项的mask图片
            Image maskImage = transform.GetChild(0).GetComponent<Image>();
            if (maskImage != null && maskImage.sprite != null)
            {
                // 获取当前选项在父对象中的索引
                int starIndex = transform.GetSiblingIndex();
                
                Debug.Log($"点击了选项 {starIndex}，贴图名称：{maskImage.sprite.name}");
                
                GameEvent.OnFinishSelectingPlanet?.Invoke(maskImage);
                
                
                // 将选中的星球数据保存到静态管理器
                StarDataManager.SetSelectedStar(maskImage.sprite, starIndex);
                
                // 通过UIManager激活Canvas StarInteract
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowStarInteractCanvas();
                }
                else
                {
                    Debug.LogError("UIManager.Instance为空！");
                }
            }
        }
    }
}




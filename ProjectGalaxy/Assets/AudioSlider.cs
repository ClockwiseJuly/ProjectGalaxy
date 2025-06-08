using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [Header("UI References")]
    public Image volumeFill;  // 使用FillAmount的Image组件
    public RectTransform dragArea;  // 可拖拽区域

    void Update()
    {
        if (Input.GetMouseButton(0) && 
            RectTransformUtility.RectangleContainsScreenPoint(dragArea, Input.mousePosition))
        {
            // 获取鼠标在dragArea中的局部坐标
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dragArea, 
                Input.mousePosition, 
                null, 
                out localPoint);

            // 计算fillAmount (0到1之间)
            float fillAmount = Mathf.InverseLerp(
                dragArea.rect.xMin, 
                dragArea.rect.xMax, 
                localPoint.x);

            // 更新UI和音量
            volumeFill.fillAmount = fillAmount;
            AudioListener.volume = fillAmount;
        }
    }
}

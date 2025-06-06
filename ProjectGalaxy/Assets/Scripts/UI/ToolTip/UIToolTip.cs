using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIToolTip : MonoBehaviour
{
    public TextMeshProUGUI headerField;

    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    [Header("Position Settings")] public float positiveXOffset = 235f; // 水平偏移量
    public float negativeXOffset = -60f; // 水平偏移量
    public float positiveYOffset = 120f; // 垂直偏移量
    public float negativeYOffset = -50f; // 垂直偏移量

    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled =
                (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
        }


        HandleTooltipPos();
    }


    public void SetText(string content, string header = "")
    {
        // 设置标题内容，如果标题为空则隐藏标题区域
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        // 设置正文内容
        contentField.text = content;

        // 根据内容长度决定是否启用自动换行
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;
        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
    }

    private void HandleTooltipPos()
    {
        // 获取鼠标当前位置
        Vector2 mousePosition = Input.mousePosition;

        // 计算初始偏移位置（默认右下角）
        Vector2 tooltipPosition = mousePosition + new Vector2(positiveXOffset, negativeYOffset);

        // Debug.Log("mousePosition is " + mousePosition);
        // Debug.Log("tooltipPosition is "  + tooltipPosition);


        // 获取提示框尺寸
        float tooltipWidth = rectTransform.rect.width;
        float tooltipHeight = rectTransform.rect.height;


        // 检查右边界
        if (tooltipPosition.x + positiveXOffset > Screen.width)
        {
            // 超出右边界，改为左下角显示
            //tooltipPosition.x = mousePosition.x + negativeXOffset - tooltipWidth;
            tooltipPosition.x = mousePosition.x + negativeXOffset;

            //Debug.Log(("进入右边界"));
        }

        // 检查左边界
        if (tooltipPosition.x < 0)
        {
            // 超出左边界，强制靠左显示
            tooltipPosition.x = 0;
        }

        // 检查上边界
        if (tooltipPosition.y > Screen.height)
        {
            // 超出上边界，改为下方显示
            tooltipPosition.y = mousePosition.y + negativeYOffset;
        }

        // 检查下边界
        if (tooltipPosition.y + negativeYOffset < 0)
        {
            // 超出下边界，强制靠下显示
            tooltipPosition.y = mousePosition.y + positiveYOffset;
        }

        // 应用最终位置
        //Debug.Log("Final tooltip pos is " + tooltipPosition);

        rectTransform.position = tooltipPosition;
    }
}
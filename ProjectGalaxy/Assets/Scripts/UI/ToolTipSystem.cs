using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    public static ToolTipSystem current;

    public  UIToolTip tooltip;

    private void Awake()
    {
        current = this;
    }

    public static void Show(string content , string header = "")
    {
        current.tooltip.SetText(content , header);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void ShowItem(string content , string header = "")
    {
        current.tooltip.SetText(content , header);
        current.tooltip.gameObject.SetActive(true);
        current.tooltip.rectTransform.anchoredPosition += new Vector2(20f ,-30f);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}

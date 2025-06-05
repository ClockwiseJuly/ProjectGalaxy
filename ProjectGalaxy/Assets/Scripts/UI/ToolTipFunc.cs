using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipFunc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Tween _delayTween;
    public string header;
    public string content;
    public float delayTime = 0.35f;

    public enum ToolTipType
    {
        none,
        item,
        other
    }

    public ToolTipType toolTipType;


    public void OnPointerEnter(PointerEventData eventData)
    {
        // ButtonInfo btnInfo = new ButtonInfo();
        // if (gameObject.GetComponent<ButtonInfo>() != null)
        // {
        //     btnInfo = gameObject.GetComponent<ButtonInfo>();
        // }

        string _content = "";
        string _header = "";


        

        _delayTween = DOVirtual.DelayedCall(delayTime, () =>
        {
            if (toolTipType == ToolTipType.none)
            {
                _header = header;
                _content = content;
                
                ToolTipSystem.ShowItem(_content, _header);
            }
            else if (toolTipType == ToolTipType.item)
            {
                if (gameObject.GetComponent<WareHouseItem>().hasItem)
                {
                    _header = gameObject.GetComponent<WareHouseItem>().name;
                    _content = gameObject.GetComponent<WareHouseItem>().description;
                    
                    ToolTipSystem.Show(_content, _header);
                }
                
                
            }
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.Hide();

        // 如果鼠标移出，取消延迟显示
        if (_delayTween != null && _delayTween.IsActive())
        {
            _delayTween.Kill(); // 终止延迟回调
            _delayTween = null;
        }
    }
}
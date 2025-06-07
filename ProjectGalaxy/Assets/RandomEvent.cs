using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    [System.Serializable]
    public class ImageData
    {
        public RectTransform image;          // 要移动的图片
        public Transform originalPosition;   // 原始位置物体
        public Transform targetPosition;     // 目标位置物体
    }

    [Header("Image Settings")]
    public ImageData leftImage;
    public ImageData centerImage;
    public ImageData rightImage;

    [Header("Animation Settings")]
    public float moveDuration = 1f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        
        //InitializePositions();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToTarget();
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            ReturnToOriginal();
        }
        
        
    }

    // private void InitializePositions()
    // {
    //   
    //     leftImage.originalPosition = new GameObject("LeftOriginalPos").transform;
    //     
    //
    //     centerImage.originalPosition = new GameObject("CenterOriginalPos").transform;
    //     
    //
    //     rightImage.originalPosition = new GameObject("RightOriginalPos").transform;
    // }

    // 移动到目标位置
    public void MoveToTarget()
    {

        StartCoroutine(AnimateMove(leftImage.image, leftImage.targetPosition.position));
    
        StartCoroutine(AnimateMove(centerImage.image, centerImage.targetPosition.position));

        StartCoroutine(AnimateMove(rightImage.image, rightImage.targetPosition.position));
    }

    // 返回原始位置
    public void ReturnToOriginal()
    {
        StartCoroutine(AnimateMove(leftImage.image, leftImage.originalPosition.position));
        
        
        StartCoroutine(AnimateMove(centerImage.image, centerImage.originalPosition.position));
        
        
        StartCoroutine(AnimateMove(rightImage.image, rightImage.originalPosition.position));
    }

    
    private IEnumerator AnimateMove(RectTransform image, Vector3 targetPos)
    {
        Vector3 startPos = image.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            float curveValue = moveCurve.Evaluate(t);
            
            image.position = Vector3.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }

        image.position = targetPos;
    }

}

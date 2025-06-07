using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectResources : MonoBehaviour
{
     [Header("Player Settings")]
    public RectTransform playerShip;
    public float moveSpeed = 200f;
    public float rotationSpeed = 100f;
    public float collectionRadius = 30f;

    [Header("Resource Settings")]
    public GameObject[] resourcePrefabs;
    public int maxResources = 5; // 场景中最大资源数量
    public Vector2 spawnArea = new Vector2(800, 400);
    [Range(0.1f, 1f)] public float globalScale = 0.7f; // 全局缩放

    [Header("Collection Effect")]
    public float popEffect = 1.3f; // 收集时的放大效果
    public float fadeTime = 0.3f; // 淡出时间
    

    private List<GameObject> activeResources = new List<GameObject>();
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        SpawnInitialResources();
    }

    void Update()
    {
        HandleMovement();
        CheckCollection();
    }

    void HandleMovement()
    {
        // 移动输入 (W/S或上/下箭头)
        float moveInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 
                         Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? -1f : 0f;
        
        // 旋转输入 (A/D或左/右箭头)
        float rotateInput = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1f : 
                           Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? -1f : 0f;

        // 应用移动和旋转
        playerShip.Rotate(0, 0, rotateInput * rotationSpeed * Time.deltaTime);
        playerShip.anchoredPosition += moveInput * moveSpeed * Time.deltaTime * (Vector2)playerShip.up;

        // 限制移动范围
        playerShip.anchoredPosition = new Vector2(
            Mathf.Clamp(playerShip.anchoredPosition.x, -spawnArea.x/2, spawnArea.x/2),
            Mathf.Clamp(playerShip.anchoredPosition.y, -spawnArea.y/2, spawnArea.y/2)
        );
    }

    void SpawnInitialResources()
    {
        for (int i = 0; i < maxResources; i++)
        {
            SpawnNewResource();
        }
    }

    void SpawnNewResource()
    {
        if (activeResources.Count >= maxResources) return;

        // 随机选择一个预制体 (0-5对应6种资源)
        int randomIndex = Random.Range(0, resourcePrefabs.Length);
        GameObject newResource = Instantiate(resourcePrefabs[randomIndex], transform);
        
        // 设置缩放
        newResource.transform.localScale = Vector3.one * globalScale;
        
        // 随机位置
        newResource.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            Random.Range(-spawnArea.x/2, spawnArea.x/2),
            Random.Range(-spawnArea.y/2, spawnArea.y/2)
        );

        activeResources.Add(newResource);
    }

    void CheckCollection()
    {
        for (int i = activeResources.Count - 1; i >= 0; i--)
        {
            GameObject resource = activeResources[i];
            if (resource == null) continue;

            float distance = Vector2.Distance(
                playerShip.anchoredPosition,
                resource.GetComponent<RectTransform>().anchoredPosition
            );

            // 使用预制体自带的碰撞大小
            float resourceSize = resource.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
            if (distance < collectionRadius + resourceSize)
            {
                CollectResource(resource);
            }
        }
    }

    void CollectResource(GameObject resource)
    {
        activeResources.Remove(resource);
        
        // 播放音效
        //AudioManager.Instance.PlaySFX();

        // 播放收集动画
        Sequence anim = DOTween.Sequence();
        anim.Append(resource.transform.DOScale(popEffect, fadeTime));
        anim.Join(resource.GetComponent<Image>().DOFade(0, fadeTime));
        anim.OnComplete(() => 
        {
            Destroy(resource);
            SpawnNewResource(); // 生成新资源
        });
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerShip.position, collectionRadius);
    }

    void OnDestroy()
    {
        DOTween.KillAll();
    }
}

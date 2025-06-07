using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollectResources : MonoBehaviour
{
    [Header("Player Settings")] 
    public GameObject collectPanel;
    public RectTransform playerShip;
    public float moveSpeed = 200f;
    public float rotationSpeed = 100f;
    public float collectionRadius = 30f;

    [Header("Resource Settings")] 
    public Image nowPlanet;
    public GameObject[] resourcePrefabs; // 6种不同资源预制体
    public int maxResources = 5;
    public Vector2 spawnArea = new Vector2(800, 400);
    [Range(0.1f, 1f)] public float globalScale = 0.7f;

    [Header("Collection Effect")]
    public float popEffect = 1.3f;
    public float fadeTime = 0.3f;

    private List<GameObject> activeResources = new List<GameObject>();

    private bool isGameActive = false;

    void Awake()
    {
        
    }

    // 外部调用此函数启动游戏
    public void StartResourceGame()
    {
        if (isGameActive) return;
        
        collectPanel.SetActive(true);
        InitializeGame();
    }

    void InitializeGame()
    {
        // 清理可能残留的资源
        CleanupResources();
        
        // 重置飞船位置
        playerShip.anchoredPosition = new Vector2(0,-300);
        playerShip.rotation = Quaternion.identity;
        
        // 生成初始资源
        SpawnInitialResources();
        
        isGameActive = true;
    }

    void OnDisable()
    {
        EndGame();
    }

    void EndGame()
    {
        if (!isGameActive) return;
        
        collectPanel.SetActive(false);
        CleanupResources();
        DOTween.KillAll();
        isGameActive = false;
    }

    void CleanupResources()
    {
        foreach (var resource in activeResources)
        {
            if (resource != null)
            {
                Destroy(resource);
            }
        }
        activeResources.Clear();
    }

    void Update()
    {
        if (!isGameActive) return;
        
        HandleMovement();
        CheckCollection();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 
                         Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? -1f : 0f;
        
        float rotateInput = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1f : 
                           Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? -1f : 0f;

        playerShip.Rotate(0, 0, rotateInput * rotationSpeed * Time.deltaTime);
        playerShip.anchoredPosition += moveInput * moveSpeed * Time.deltaTime * (Vector2)playerShip.up;

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

        int randomIndex = Random.Range(0, resourcePrefabs.Length);
        GameObject newResource = Instantiate(resourcePrefabs[randomIndex], transform);
        newResource.transform.localScale = Vector3.one * globalScale;
        
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
        
        //播放音效
        //AudioManager.Instance.PlaySFX();

        Sequence anim = DOTween.Sequence();
        anim.Append(resource.transform.DOScale(popEffect, fadeTime));
        anim.Join(resource.GetComponent<Image>().DOFade(0, fadeTime));
        anim.OnComplete(() => 
        {
            Destroy(resource);
            SpawnNewResource();
        });
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerShip.position, collectionRadius);
    }
}

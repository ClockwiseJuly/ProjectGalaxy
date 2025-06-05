using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceMeteorAvoidanceGame: MonoBehaviour
{
    [Header("Player Settings")]
    public RectTransform playerShip;
    public float moveSpeed = 300f;
    public float rotationSpeed = 180f;
    
    [Header("Meteor Settings")]
    public RectTransform meteorPrefab;
    public float spawnInterval = 1f;
    public float meteorSpeed = 200f;
    
    [Header("Game Settings")]
    public float gameDuration = 60f;
    
    [Header("UI References")]
    public RectTransform gamePanel; // 游戏面板边界
    public Slider timeSlider;
    public TextMeshProUGUI resultText;
    public GameObject gameOverPanel;
    
    private float remainingTime;
    private bool isPlaying;
    private bool playerHit;
    private float spawnTimer;
    private List<RectTransform> activeMeteors = new List<RectTransform>();
    private Vector2 panelSize;

    private void Awake()
    {
        // 计算面板实际大小
        panelSize = new Vector2(
            gamePanel.rect.width * gamePanel.localScale.x,
            gamePanel.rect.height * gamePanel.localScale.y
        );
        
        playerShip.gameObject.SetActive(false);
        timeSlider.gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        StartGame();
    }

    private void OnDisable()
    {
        EndGame(false);
    }

    private void StartGame()
    {
        remainingTime = gameDuration;
        playerHit = false;
        
        timeSlider.maxValue = gameDuration;
        timeSlider.value = remainingTime;
        
        // 初始化飞船位置（面板中央）
        playerShip.anchoredPosition = Vector2.zero;
        playerShip.rotation = Quaternion.identity;
        playerShip.gameObject.SetActive(true);
        
        timeSlider.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
        
        // 清除之前生成的陨石
        foreach (var meteor in activeMeteors)
        {
            if (meteor != null) Destroy(meteor.gameObject);
        }
        activeMeteors.Clear();
        
        isPlaying = true;
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (!isPlaying) return;
        
        // 游戏计时
        remainingTime -= Time.deltaTime;
        timeSlider.value = remainingTime;
        
        if (remainingTime <= 0)
        {
            EndGame(true);
            return;
        }
        
        // 玩家控制
        HandlePlayerInput();
        
        // 生成陨石
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnMeteor();
            spawnTimer = spawnInterval;
        }
        
        // 移动陨石
        MoveMeteors();
    }

    private void HandlePlayerInput()
    {
        // 前进后退 (W/S或上/下箭头)
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveInput = -1f;
        
        // 旋转 (A/D或左/右箭头)
        float rotateInput = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) rotateInput = 1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) rotateInput = -1f;
        
        // 应用移动和旋转
        playerShip.Rotate(0, 0, rotateInput * rotationSpeed * Time.deltaTime);
        playerShip.anchoredPosition += moveInput * moveSpeed * Time.deltaTime * (Vector2)playerShip.up;
        
        // 限制在面板范围内
        Vector2 clampedPosition = playerShip.anchoredPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -panelSize.x/2, panelSize.x/2);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -panelSize.y/2, panelSize.y/2);
        playerShip.anchoredPosition = clampedPosition;
    }

    private void SpawnMeteor()
    {
        RectTransform meteor = Instantiate(meteorPrefab, gamePanel);
        meteor.anchoredPosition = GetRandomEdgePosition();
        activeMeteors.Add(meteor);
    }

    private Vector2 GetRandomEdgePosition()
    {
        int edge = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;
        
        switch (edge)
        {
            case 0: // 上边缘
                spawnPos = new Vector2(
                    Random.Range(-panelSize.x/2, panelSize.x/2),
                    panelSize.y/2 + 50f
                );
                break;
            case 1: // 右边缘
                spawnPos = new Vector2(
                    panelSize.x/2 + 50f,
                    Random.Range(-panelSize.y/2, panelSize.y/2)
                );
                break;
            case 2: // 下边缘
                spawnPos = new Vector2(
                    Random.Range(-panelSize.x/2, panelSize.x/2),
                    -panelSize.y/2 - 50f
                );
                break;
            case 3: // 左边缘
                spawnPos = new Vector2(
                    -panelSize.x/2 - 50f,
                    Random.Range(-panelSize.y/2, panelSize.y/2)
                );
                break;
        }
        
        return spawnPos;
    }

    private void MoveMeteors()
    {
        for (int i = activeMeteors.Count - 1; i >= 0; i--)
        {
            RectTransform meteor = activeMeteors[i];
            if (meteor == null)
            {
                activeMeteors.RemoveAt(i);
                continue;
            }
            
            // 向玩家移动
            Vector2 direction = (playerShip.anchoredPosition - meteor.anchoredPosition).normalized;
            meteor.anchoredPosition += direction * meteorSpeed * Time.deltaTime;
            
            // 检查碰撞（简单的距离检测）
            if (Vector2.Distance(meteor.anchoredPosition, playerShip.anchoredPosition) < 50f)
            {
                PlayerHit();
                Destroy(meteor.gameObject);
                activeMeteors.RemoveAt(i);
                continue;
            }
            
            // 检查是否离开面板范围
            if (Mathf.Abs(meteor.anchoredPosition.x) > panelSize.x/2 + 100f ||
                Mathf.Abs(meteor.anchoredPosition.y) > panelSize.y/2 + 100f)
            {
                Destroy(meteor.gameObject);
                activeMeteors.RemoveAt(i);
            }
        }
    }

    private void PlayerHit()
    {
        if (playerHit || !isPlaying) return;
        
        playerHit = true;
        EndGame(false);
    }

    private void EndGame(bool win)
    {
        isPlaying = false;
        
        playerShip.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        resultText.text = win ? "You Win!" : "Game Over!";
        
        // 3秒后自动禁用
        Invoke("DisableGame", 3f);
    }

    private void DisableGame()
    {
        gameOverPanel.SetActive(false);
        gameObject.SetActive(false);
    }
}

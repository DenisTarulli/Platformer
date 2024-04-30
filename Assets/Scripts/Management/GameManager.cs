using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("GameOver")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [HideInInspector] public bool gameIsOver;

    [Header("UI")]
    [HideInInspector] public int currentGemCount;
    [SerializeField] private int maxGemCount;
    [SerializeField] private TextMeshProUGUI gemCountText;
    [SerializeField] private TextMeshProUGUI timerText;
    private float time;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
        TimeUpdate();
    }

    public void GameOver()
    {
        gameIsOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void AddGem()
    {
        currentGemCount++;
        gemCountText.text = $"{currentGemCount:D2}/{maxGemCount:D2}";
    }
    private void TimeUpdate()
    {
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

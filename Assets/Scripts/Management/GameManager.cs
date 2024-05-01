using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private float baseScore;
    [SerializeField] private GameObject scoreText;
    private float score;

    [Header("GameOver")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [HideInInspector] public bool gameIsOver;

    [Header("UI")]
    [HideInInspector] public int currentGemCount;
    [SerializeField] private int maxGemCount;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TextMeshProUGUI gemCountText;
    [SerializeField] private TextMeshProUGUI timerText;
    private float time;

    [Header("Script references")]
    private PlayerCombat playerCombat;

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

    private void Start()
    {
        Time.timeScale = 1f;
        playerCombat = FindObjectOfType<PlayerCombat>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        TimeUpdate();
    }

    public void GameOver()
    {
        score = (baseScore * (1 + currentGemCount)) / (time / 2);       
        Cursor.lockState = CursorLockMode.None;

        gameUI.SetActive(false);
        gameIsOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        if (playerCombat.currentHealth <= 0)
            loseText.SetActive(true);
        else
        {
            winText.SetActive(true);
            scoreText.SetActive(true);
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = $"Final score: {Mathf.FloorToInt(score)}";
        }
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

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
}

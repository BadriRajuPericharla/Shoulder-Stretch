using UnityEngine;
using System;
public class ScoringSystem : MonoBehaviour
{
    public static ScoringSystem Instance { get; private set; }
    public event Action<int> OnScoreChanged;
    [SerializeField] private GameStateManager gameManager;
    [SerializeField] private InputSystem inputSystem;
    [SerializeField] private int killPoints = 10;
    private int currentScore;
    private int highScore;
    public int CurrentScore => currentScore;
    public int HighScore => highScore;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
    private void Start()
    {
        if (gameManager == null) gameManager = FindFirstObjectByType<GameStateManager>();
        if (gameManager != null) gameManager.OnStateChanged += HandleStateChange;
    }
    private void OnDestroy()
    {
        if (gameManager != null) gameManager.OnStateChanged -= HandleStateChange;
    }
    public  void AddKillPoints()
    {
        currentScore += killPoints ; 
        OnScoreChanged?.Invoke(currentScore); 
    }



    private void HandleStateChange(GameState state)
    {
        if (state == GameState.Running) { currentScore = 0;OnScoreChanged?.Invoke(0); }
        else if (state == GameState.Dashboard) SaveScore();
    }
    private void SaveScore() { if (currentScore > highScore) { highScore = currentScore; PlayerPrefs.SetInt("HighScore", highScore); } }
}
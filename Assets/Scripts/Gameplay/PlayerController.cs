using System;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnAmmoChanged;
    public event Action<bool> OnShieldStatusChanged;
    public event Action OnPlayerDeath;
    [SerializeField] private GameStateManager gameManager;
    [SerializeField] private DifficultyScaler difficultyScaler;
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private int maxAmmo = 10;
    public int currentAmmo;
    [SerializeField] private float shieldDuration = 2.0f;
    private bool isShieldActive;
    private float shieldTimer;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentAmmo => currentAmmo;
    public bool IsShieldActive => isShieldActive;

    public UIManager uiManager;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    private void Start()
    {
        if (gameManager == null) gameManager = FindFirstObjectByType<GameStateManager>();
        if (difficultyScaler == null) difficultyScaler = FindFirstObjectByType<DifficultyScaler>();
        ResetPlayer();
        if (gameManager != null) gameManager.OnStateChanged += HandleStateChange;
    }
    private void OnDestroy() { if (gameManager != null) gameManager.OnStateChanged -= HandleStateChange; }
    private void HandleStateChange(GameState newState) { if (newState == GameState.Running) ResetPlayer(); }
    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        currentAmmo = difficultyScaler?.InitialAmmo ?? 5;
        isShieldActive = false;
        transform.position = new Vector3(0, 0, 0);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnAmmoChanged?.Invoke(currentAmmo);
        OnShieldStatusChanged?.Invoke(false);
    }
    private void Update()
    {
        if (gameManager == null || !gameManager.IsPlaying) return;

        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0) { isShieldActive = false; OnShieldStatusChanged?.Invoke(false); }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isShieldActive) return;
        currentHealth -= damage;
        uiManager.ShowDamageBlink();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0) Die();
    }
    private void Die()
    {
        OnPlayerDeath?.Invoke();
        gameManager?.EndGame();
    }
    public void AddAmmo(int amount) { currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo); OnAmmoChanged?.Invoke(currentAmmo); }
    public bool UseAmmo()
    {
        if (currentAmmo > 0) { currentAmmo--; OnAmmoChanged?.Invoke(currentAmmo); return true; }
        return false;
    }
    public void ActivateShield()
    {
        isShieldActive = true;
        shieldTimer = shieldDuration;
        OnShieldStatusChanged?.Invoke(true);
    }
    public void AmmoReload(int amount)
    {
        currentAmmo = Mathf.Min(CurrentAmmo+ amount, maxAmmo);
        OnAmmoChanged?.Invoke(currentAmmo);
    }
}
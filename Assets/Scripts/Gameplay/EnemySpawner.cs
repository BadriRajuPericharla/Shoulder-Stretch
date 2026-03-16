using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameStateManager gameManager;
    [SerializeField] private DifficultyScaler difficultyScaler;
    [SerializeField] private PlayerController player;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnWidth = 8f;
    [SerializeField] private float spawnDistance = 30f;
   
    
    private List<Enemy> pool = new List<Enemy>();
    private float spawnTimer;
    private void Start()
    {
        if (gameManager == null) gameManager = FindFirstObjectByType<GameStateManager>();
        if (difficultyScaler == null) difficultyScaler = FindFirstObjectByType<DifficultyScaler>();
        if (player == null) player = FindFirstObjectByType<PlayerController>();
        
    }
    private void Update()
    {
        if (gameManager == null || !gameManager.IsPlaying) return;
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = difficultyScaler.EnemySpawnInterval;
        }
    }
    private void SpawnEnemy()
    {
        Enemy enemy = GetFromPool();
        Vector3 pos = player.transform.position + Vector3.forward * spawnDistance + Vector3.right * Random.Range(-spawnWidth, spawnWidth);
        enemy.transform.position = pos;
        enemy.Initialize(player.transform);
        
    }
    private Enemy GetFromPool()
    {
        foreach (var e in pool) if (!e.gameObject.activeInHierarchy) return e;
        Enemy newEnemy = Instantiate(enemyPrefab, transform);
        pool.Add(newEnemy);
        return newEnemy;
    }
}
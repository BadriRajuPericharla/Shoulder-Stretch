using System.Collections.Generic;

using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameStateManager gameManager;
    [SerializeField] private DifficultyScaler difficultyScaler;
    [SerializeField] private PlayerController player;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnWidth = 8f;
    [SerializeField] private float spawnDistance = 80f;
    [SerializeField]private int Zombies=10;
    [SerializeField] private float SpawnDuration=10f;
    private int spawnedCount;
    private int currentwave=1;
    private float spawnInterval;
  
   
    
    public List<Enemy> activeEnemies = new List<Enemy>();
    private float spawnTimer;
    private void Start()
    {
        if (gameManager == null) gameManager = FindFirstObjectByType<GameStateManager>();
        if (difficultyScaler == null) difficultyScaler = FindFirstObjectByType<DifficultyScaler>();
        if (player == null) player = FindFirstObjectByType<PlayerController>();
        spawnInterval=SpawnDuration/Zombies;
    }
    private void Update()
    {
        if (gameManager == null || !gameManager.IsPlaying) return;
        spawnTimer -= Time.deltaTime;
        if (spawnedCount >= Zombies)
        {
            if (currentwave == 1)
            {
                
                Zombies += 5;
                spawnInterval = SpawnDuration / Zombies;
                spawnedCount = 0;
                spawnTimer = 15f;
                currentwave++;
                return;
            }
            else if (currentwave == 2)
            {
                
                Zombies += 5;
                spawnInterval = SpawnDuration / Zombies;
                spawnedCount = 0;
                spawnTimer = 15f;
                currentwave++;
                return;
            }
            else return;
        }
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }
    private void SpawnEnemy()
    {
        Enemy enemy = GetFromPool();
        //float xOffset;

        //if (Random.value > 0.5f)
        //    xOffset = Random.Range(2f, spawnWidth);     
        //else
        //    xOffset = Random.Range(-spawnWidth, -2f);
        //Vector3 pos = player.transform.position + Vector3.forward * spawnDistance + Vector3.right * xOffset;

        bool spwanLeft = Random.value < 0.5;
        float laneX = spwanLeft ? -3f : 3f;
        Vector3 pos = new Vector3(laneX, 0, spawnDistance);

        enemy.transform.position = pos;
        enemy.lane = spwanLeft? Enemy.Lane.Left : Enemy.Lane.Right;
        enemy.Initialize(player.transform);
        spawnedCount++;
    }
    private Enemy GetFromPool()
    {
        foreach (var e in activeEnemies) if (!e.gameObject.activeInHierarchy) return e;
        Enemy newEnemy = Instantiate(enemyPrefab, transform);
        activeEnemies.Add(newEnemy);
        return newEnemy;
    }
}
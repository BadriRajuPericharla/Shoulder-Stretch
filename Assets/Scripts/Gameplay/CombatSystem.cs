using UnityEngine;
public class CombatSystem : MonoBehaviour
{
    [SerializeField] private InputSystem inputSystem;
    [SerializeField] private PlayerController player;
    [SerializeField] private float punchRange = 4f;
    [SerializeField] private int punchDamage = 50;
    [SerializeField] private float shootRange = 50f;
    [SerializeField] private int shootDamage = 100;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletTarget;
    [SerializeField] private EnemySpawner spawner;
    private void Start()
    {
        if (inputSystem == null) inputSystem = FindFirstObjectByType<InputSystem>();
        if (player == null) player = GetComponent<PlayerController>();
        if (spawner == null) spawner = FindFirstObjectByType<EnemySpawner>();
        if (inputSystem != null) inputSystem.OnActionPerformed += HandleAction;
    }
    private void OnDestroy() { if (inputSystem != null) inputSystem.OnActionPerformed -= HandleAction; }
    private void HandleAction(ActionType action, bool success)
    {
        if (!success) return;
        switch (action)
        {
            case ActionType.leftShoot: PerforLeftShoot(); break;
            case ActionType.rightShoot: PerforRightShoot(); break;
        }
    }

    
    private void PerforLeftShoot()
    {
        if (player == null || !player.UseAmmo()) return;

        ShootAtLane(Enemy.Lane.Left);
    }

    private void PerforRightShoot()
    {
        if (player == null || !player.UseAmmo()) return;

        ShootAtLane(Enemy.Lane.Right);

    }


    private void ShootAtLane(Enemy.Lane targetLane)
    {
        Enemy closest = null;
        float minDist = shootRange;

        foreach(Enemy enemy in spawner.activeEnemies)
        {
            if (!enemy.gameObject.activeInHierarchy) continue;
            if(enemy.lane != targetLane) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if(dist < minDist)
            {
                closest = enemy;
                minDist = dist;
            }
        }

        if (closest != null) closest.TakeDamage(shootDamage);

    }
}
using System;
using UnityEngine;
using System.Threading;
public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnEnemyDeath;
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 20;
    private int currentHealth;
    private Transform player;
    private PlayerController playerCtrl;
    private GameObject HealthPack;
    void Awake()
    {
        HealthPack=transform.Find("HealthPack").gameObject;
    }
    public void Initialize(Transform target)
    {
        player = target;
        playerCtrl = player.GetComponent<PlayerController>();
        currentHealth = health;
        if (HealthPack != null)
        {
            HealthPack.SetActive(UnityEngine.Random.value<0.3f);
        }
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (player == null) return;
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.position) < 1.5f) { Attack(); }
        if (player.position.z > transform.position.z + 8f)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    private void Attack()
    {
        if (playerCtrl) playerCtrl.TakeDamage(damage);
        Die();
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }
    private void Die()
    {
        if (HealthPack != null && HealthPack.activeSelf)
        {
            playerCtrl.Heal(30);  
        }
        OnEnemyDeath?.Invoke(this);
        gameObject.SetActive(false);
    }
}
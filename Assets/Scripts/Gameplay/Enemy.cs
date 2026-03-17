using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnEnemyDeath;
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 20;
    [SerializeField]private float packSpeed=8f;
    private int currentHealth;
    private Transform player;
    private bool movePack=false;
    private PlayerController playerCtrl;
    private GameObject HealthPack;
    private Transform packParent;
    private Vector3 packLocalPos;
    void Awake()
    {
        HealthPack=transform.Find("HealthPack").gameObject;
        packParent = HealthPack.transform.parent;
        packLocalPos = HealthPack.transform.localPosition;

    }
    public void Initialize(Transform target)
    {
        player = target;
        playerCtrl = player.GetComponent<PlayerController>();
        currentHealth = health;
        if (HealthPack != null)
        {
            HealthPack.SetActive(false);
            HealthPack.SetActive(UnityEngine.Random.value<0.3f);
        }
        gameObject.SetActive(true);
    }
    private void Update()
    {
        
        if (movePack && HealthPack != null)
        {
            HealthPack.transform.position = Vector3.MoveTowards(
                HealthPack.transform.position,
                player.position,
                packSpeed * Time.deltaTime
            );

            if (Vector3.Distance(HealthPack.transform.position, player.position) < 0.5f)
            {
                playerCtrl.Heal(30);
                HealthPack.transform.SetParent(packParent);
                HealthPack.transform.localPosition = packLocalPos;
                HealthPack.SetActive(false);
                movePack = false;
                OnEnemyDeath?.Invoke(this);
                gameObject.SetActive(false);
            }
            return;
        }
        
        if (player == null) return;
        if (player.position.z > transform.position.z + 8f)
        {
            
            gameObject.SetActive(false);
            return;
        }
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.position) < 1.5f) { Attack(); }
        
    }
    private void Attack()
    {
        if (playerCtrl) playerCtrl.TakeDamage(damage);
        gameObject.SetActive(false);
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
            HealthPack.transform.SetParent(null);
            HealthPack.SetActive(true);
            movePack=true;
            return;
             
        }
        OnEnemyDeath?.Invoke(this);
        gameObject.SetActive(false);
    }
}
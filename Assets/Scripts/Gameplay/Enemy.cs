using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnEnemyDeath;
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 20;
    [SerializeField]private float packSpeed=16f;
    [SerializeField]private ParticleSystem deathParticle;
    private int currentHealth;
    private Transform player;
    private bool movePack=false;
    private PlayerController playerCtrl;
    private GameObject AmmoPack;
    private Transform packParent;
    private Vector3 packLocalPos;

    public enum Lane { Left, Right};
    public Lane lane;

    Animator animator;

    private bool hasAttacked = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Awake()
    {
        AmmoPack=transform.Find("AmmoPack").gameObject;
        packParent = AmmoPack.transform.parent;
        packLocalPos = AmmoPack.transform.localPosition;

    }
    public void Initialize(Transform target)
    {
        player = target;
        playerCtrl = player.GetComponent<PlayerController>();
        currentHealth = health;

        hasAttacked = false;

        if (AmmoPack != null)
        {
            AmmoPack.SetActive(false);
            AmmoPack.SetActive(UnityEngine.Random.value<0.3f);
        }
        gameObject.SetActive(true);
    }
    private void Update()
    {
        
        if (movePack && AmmoPack != null)
        {
            AmmoPack.transform.position = Vector3.MoveTowards(AmmoPack.transform.position,player.position,packSpeed * Time.deltaTime);
            if (Vector3.Distance(AmmoPack.transform.position, player.position) < 0.5f)
            {
                playerCtrl.AmmoReload(5);
                AmmoPack.transform.SetParent(packParent);
                AmmoPack.transform.localPosition = packLocalPos;
                AmmoPack.SetActive(false);
                movePack = false;
                OnEnemyDeath?.Invoke(this);
                gameObject.SetActive(false);
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (player == null) return;
        Vector3 direction = (player.position - transform.position).normalized;
        transform.LookAt(player);
        

        if (distance > 3f)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            
            if (!hasAttacked)
            {
                hasAttacked = true;
                Attack();
            }
        }
    }
    private void Attack()
    {
        if (playerCtrl) playerCtrl.TakeDamage(damage);
        animator.SetTrigger("IsAttack");
        
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        deathParticle.Play();
        if (currentHealth <= 0) Die();
    }
    private void Die()
    {
        if (AmmoPack != null && AmmoPack.activeSelf)
        {
            AmmoPack.transform.SetParent(null);
            AmmoPack.SetActive(true);
            movePack=true;
            return;
             
        }
        Invoke(nameof(DisableEnemy), 2f);
    }
    void DisableEnemy()
    {
        OnEnemyDeath?.Invoke(this);
        gameObject.SetActive(false);
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class mob : MonoBehaviour, IDamageable
{
    public enum State { Idle, Walk, Attack, DMG, Death }

    [Header("스탯")]
    public int maxHP = 20;
    public float moveSpeed = 2f;

    [Header("순찰 설정")]
    public float patrolRadius = 3f;
    public float patrolChangeInterval = 2f;

    [Header("공격 설정")]
    public float chaseRange = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;

    private Vector2 patrolTarget;
    private float patrolTimer;
    private int currentHP;
    private bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        currentHP = maxHP;
        SetNewPatrolTarget();
    }

    void Update()
    {
        if (isDead) return;

        float dist = player ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;

        if (dist < chaseRange)
        {
            animator.SetBool("walk", true);
            ChasePlayer();
        }
        else
        {
            animator.SetBool("walk", true);
            Patrol();
        }
    }


    void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer > patrolChangeInterval)
        {
            SetNewPatrolTarget();
            patrolTimer = 0;
        }

        Vector2 dir = (patrolTarget - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x, rb.linearVelocity.y) * moveSpeed;
        transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    void SetNewPatrolTarget()
    {
        patrolTarget = (Vector2)transform.position + Random.insideUnitCircle * patrolRadius;
    }

    void ChasePlayer()
    {
        if (!player) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x, rb.linearVelocity.y) * moveSpeed;
        transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    public void TakeDamage(int damage, Vector2 hitPoint, Vector2 hitDirection)
    {
        if (isDead) return;

        currentHP -= damage;
        animator.SetTrigger("DMG");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("death");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 2f);
    }
}

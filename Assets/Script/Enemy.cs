using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int maxHP = 3;
    public float moveSpeed = 1.5f;
    public float chaseRange = 6f;

    int _hp;
    bool _dead;

    Rigidbody2D _rb;
    SpriteRenderer _spr;
    Animator _anim;
    Transform _player;          

    readonly float _yDeathLimit = -20f;

    void Awake()
    {
        _hp = maxHP;
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (_dead || !_player) return;

        float dist = Vector2.Distance(_player.position, transform.position);
        if (dist < chaseRange)
        {
            Vector2 dir = (_player.position - transform.position).normalized;
            if (dir.x != 0) _spr.flipX = dir.x < 0;

            _rb.linearVelocity = new Vector2(dir.x * moveSpeed, _rb.linearVelocity.y);

            _anim?.SetBool("isWalking", true);
        }
        else
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            _anim?.SetBool("isWalking", false);
        }

        if (transform.position.y < _yDeathLimit)
            Die();
    }

    #region IDamageable 
    public void TakeDamage(int dmg, Vector2 hitPoint, Vector2 hitDir)
    {
        if (_dead) return;

        _hp -= dmg;
        _anim?.SetTrigger("Hit");

        _rb.AddForce(hitDir.normalized * 5f, ForceMode2D.Impulse);

        if (_hp <= 0)
            Die();
    }
    #endregion
    void Die()
    {
        if (_dead) return;
        _dead = true;

        // 사망 애니메이션
        if (_anim && _anim.enabled)
            _anim.SetTrigger("Die");

        // 물리 해제: 중력·충돌 모두 끔
        _rb.linearVelocity = Vector2.zero;
        _rb.simulated = false;             // Rigidbody2D 시뮬레이션 꺼버리기
        GetComponent<Collider2D>().enabled = false;

        // 원하는 시간 뒤 파괴
        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            TakeDamage(1,
                       other.ClosestPoint(transform.position),
                       transform.position - other.transform.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

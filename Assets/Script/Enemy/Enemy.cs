using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    public enum State { Patrol, Chase, Dead }
    [Header("콜백")]
    public Action onDeath;

    [Header("레벨 & 보상 설정")]
    [Tooltip("몬스터 기본 레벨 (1부터)")]
    public int level = 1;
    [Tooltip("레벨 당 추가 최대 HP")]
    public int hpPerLevel = 5;
    [Tooltip("레벨 당 추가 경험치 보상")]
    public int expPerLevel = 20;
    [Tooltip("레벨 당 추가 골드 보상")]
    public int goldPerLevel = 1;
    [Tooltip("기본 경험치 보상")]
    public int baseExpReward = 50;
    [Tooltip("기본 골드 보상")]
    public int baseGoldReward = 5;

    [Header("기본 스탯")]
    public int baseMaxHP = 10;
    public float moveSpeed = 1.5f;
    public float chaseRange = 6f;

    [Header("순찰 설정")]
    public State currentState = State.Patrol;
    public float patrolRadius = 3f;
    public float patrolSpeed = 1f;
    public float patrolChangeInterval = 2f;

    [Header("드롭 설정")]
    public DropTable dropTable;
    public GameObject goldPrefab;   // 금화 프리팹

    [Header("낙사 임계치")]
    public float yDeathLimit = -20f;

    // 내부 계산된 값
    private int maxHP;
    private int currentHP;
    private int expReward;
    private int goldReward;

    // 컴포넌트 참조
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;
    private Animator _anim;
    private Transform _player;

    // 순찰용
    private Vector2 patrolTarget;
    private float patrolTimer;

    [Header("전투 스탯")]
    public int defense = 0;
    public int evade = 5;

    void Awake()
    {
        // 1) 레벨 기반 스케일링
        maxHP = baseMaxHP + hpPerLevel * (level - 1);
        currentHP = maxHP;
        expReward = baseExpReward + expPerLevel * (level - 1);
        goldReward = baseGoldReward + goldPerLevel * (level - 1);

        // 2) 컴포넌트 획득
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player")?.transform;

        // 3) 첫 순찰 목표 생성
        SetNewPatrolTarget();
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        // 플레이어 거리 계산
        float dist = _player
            ? Vector2.Distance(_player.position, transform.position)
            : Mathf.Infinity;

        // 상태 전환
        currentState = (dist < chaseRange) ? State.Chase : State.Patrol;

        // 상태별 행동
        if (currentState == State.Patrol) Patrol();
        else if (currentState == State.Chase) Chase();

        // 애니메이션
        bool isWalking = (currentState == State.Patrol || currentState == State.Chase);
        _anim?.SetBool("isWalking", isWalking);

        // 낙사 처리
        if (transform.position.y < yDeathLimit)
            Die();
    }

    private void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolChangeInterval)
        {
            SetNewPatrolTarget();
            patrolTimer = 0f;
        }

        Vector2 dir = (patrolTarget - (Vector2)transform.position).normalized;
        _rb.linearVelocity = new Vector2(dir.x * patrolSpeed, _rb.linearVelocity.y);
        _spr.flipX = dir.x < 0;
    }

    private void SetNewPatrolTarget()
    {
        Vector2 center = transform.position;
        Vector2 offset = UnityEngine.Random.insideUnitCircle * patrolRadius;
        patrolTarget = center + offset;
    }

    private void Chase()
    {
        if (!_player) return;
        Vector2 dir = (_player.position - transform.position).normalized;
        _rb.linearVelocity = new Vector2(dir.x * moveSpeed, _rb.linearVelocity.y);
        _spr.flipX = dir.x < 0;
    }

    #region IDamageable
    public void TakeDamage(int baseDmg, Vector2 hitPoint, Vector2 hitDir)
    {
        if (currentState == State.Dead) return;
        int playerLevel = PlayerLevel.instance?.currentLevel ?? 1;
        int levelDiff = playerLevel - level;  

        float bonusPerLevel = 0.05f;  
        float penaltyPerLevel = 0.03f;  

        float modifier = 1f;
        if (levelDiff > 0)
            modifier += levelDiff * bonusPerLevel;
        else if (levelDiff < 0)
            modifier += levelDiff * penaltyPerLevel;

        int finalDmg = Mathf.RoundToInt(baseDmg * modifier);

        int mitigated = Mathf.Max(0, finalDmg - defense);

        // 6) 체력 차감 & 히트 연출
        currentHP -= mitigated;
        _anim?.SetTrigger("Hit");

        DamageTextSpawner.I.Spawn(mitigated, hitPoint, false);
        if (mitigated == 0)
        {
            DamageTextSpawner.I.SpawnText("Miss", hitPoint);
            return;
        }
        if (currentHP <= 0)
            Die();
    }
    #endregion

    private void Die()
    {
        currentState = State.Dead;
        _anim?.SetTrigger("Die");
        _rb.linearVelocity = Vector2.zero;
        _rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        // 경험치·골드 보상
        PlayerLevel.instance?.AddExp(expReward);
        GoldManager.instance?.AddGold(goldReward);

        // 드롭 처리 (아이템 + 골드)
        TryDropItems();
        onDeath?.Invoke();
        Destroy(gameObject, 1f);
    }

    private void TryDropItems()
    {
        Collider2D enemyCol = GetComponent<Collider2D>();

        // 아이템 드롭
        if (dropTable != null)
        {
            foreach (var entry in dropTable.entries)
            {
                if (UnityEngine.Random.value < entry.dropChance)
                {
                    var go = Instantiate(dropTable.pickupPrefab,
                                         transform.position,
                                         Quaternion.identity);
                    go.GetComponent<ItemPickup>()?.Initiallize(entry.itemData);
                }
            }
        }
        // 골드 드롭: 겹치지 않도록 랜덤 오프셋
        if (goldPrefab != null)
        {
            // 1) 약간의 랜덤 X 오프셋
            Vector2 rndOffset = UnityEngine.Random.insideUnitCircle * 0.3f;

            // 2) 골드 인스턴스화
            var g = Instantiate(
                goldPrefab,
                (Vector2)transform.position + rndOffset,
                Quaternion.identity);

            // 3) 충돌 무시: 몬스터와 골드
            Collider2D goldCol = g.GetComponent<Collider2D>();
            if (enemyCol != null && goldCol != null)
                Physics2D.IgnoreCollision(enemyCol, goldCol);

            // 4) 물리 속성
            var rbG = g.GetComponent<Rigidbody2D>();
            if (rbG)
            {
                // 위쪽으로 튕기고 랜덤 좌우 방향
                Vector2 impulseDir = new Vector2(
                    UnityEngine.Random.Range(-0.5f, 0.5f),
                    1f).normalized;

                float impulseStrength = 5f;
                rbG.AddForce(impulseDir * impulseStrength,
                             ForceMode2D.Impulse);
            }
        }
    }

    // 디버그용 깃발 범위 그리기
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

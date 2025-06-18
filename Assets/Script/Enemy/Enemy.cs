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


    [Header("아이템 드랍테이블")]
    public ItemData DropItemData;
    public int dropQuantity = 1;


    [Header("밀격 설정")]
    private float knockbackTimer = 0f;
    public float knockbackDuration = 0.2f;

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
        _spr = GetComponentInChildren<SpriteRenderer>();
        if (_spr == null)
        {
            _spr = GetComponent<SpriteRenderer>();
        }
        _anim = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player")?.transform;

        // 3) 첫 순찰 목표 생성
        SetNewPatrolTarget();
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        //밀격 컴포넌트
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            return; // 밀격 중엔 이동 X
        }


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

        //밀격
        float knockbackForce = 5f;
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(hitDir.normalized * knockbackForce, ForceMode2D.Impulse);
        knockbackTimer = knockbackDuration;



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

     void Die()
    {
        currentState = State.Dead;
        _anim?.SetTrigger("Die");
        _rb.linearVelocity = Vector2.zero;
        _rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        PlayerLevel.instance?.AddExp(expReward);
        GoldManager.instance?.AddGold(goldReward);
        QuestManager.instance?.UpdateQuestProgress(gameObject.name);
        TryDropItems();
        onDeath?.Invoke();
        Destroy(gameObject, 1f);
    }

     void TryDropItems()
    {
        Collider2D enemyCol = GetComponent<Collider2D>();

        if (dropTable != null)
        {
            foreach (var entry in dropTable.entries)
            {
                if (entry.itemData.type == ItemType.Quest)
                {
                    var quest = QuestManager.instance.activeQuests.Find(q =>
                        q.data.conditionType == QuestConditionType.CollectItem &&
                        q.data.targetName == entry.itemData.itemName &&
                        q.state == QuestState.InProgress);

                    if (quest == null)
                        continue; 

                    int owned = Inventory.instance.GetItemCount(entry.itemData.itemName);
                    if (owned >= quest.data.requiredAmount)
                        continue;
                }

                if (UnityEngine.Random.value < entry.dropChance)
                {
                    var go = Instantiate(dropTable.pickupPrefab,
                                         transform.position,
                                         Quaternion.identity);
                    go.GetComponent<ItemPickup>()?.Initiallize(entry.itemData);
                }
            }
        }
        if (goldPrefab != null)
        {
            Vector2 rndOffset = UnityEngine.Random.insideUnitCircle * 0.3f;

            var g = Instantiate(
                goldPrefab,
                (Vector2)transform.position + rndOffset,
                Quaternion.identity);

            Collider2D goldCol = g.GetComponent<Collider2D>();
            if (enemyCol != null && goldCol != null)
                Physics2D.IgnoreCollision(enemyCol, goldCol);

            var rbG = g.GetComponent<Rigidbody2D>();
            if (rbG)
            {
                Vector2 impulseDir = new Vector2(
                    UnityEngine.Random.Range(-0.5f, 0.5f),
                    1f).normalized;

                float impulseStrength = 5f;
                rbG.AddForce(impulseDir * impulseStrength,
                             ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}

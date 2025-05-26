using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SkillHitbox : MonoBehaviour
{
    public int damage = 5;
    public float duration = 0.3f;

    [Header("VFX")]
    public GameObject bloodKnightVFX;
    public Transform vfxSpawnPoint;

    [Header("Range Scaling")]
    [Tooltip("레벨 10부터 적용, 레벨당 몇 퍼센트씩 범위 증가")]
    public float percentPerLevel = 0.05f;

    Collider2D hitboxCollider;

    void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
    
        int playerLvl = PlayerLevel.instance.currentLevel;
        if (playerLvl > 10)
        {
            float extra = (playerLvl - 10) * percentPerLevel;
            Vector2 baseSize = GetBaseColliderSize();
            SetColliderSize(baseSize * (1 + extra));
        }

        if (playerLvl >= 10 && bloodKnightVFX != null)
            SpawnVFX();

        Destroy(gameObject, duration);
    }

    Vector2 GetBaseColliderSize()
    {
        if (hitboxCollider is BoxCollider2D box)
            return box.size;
        if (hitboxCollider is CircleCollider2D circ)
            return Vector2.one * circ.radius * 2f;
        return Vector2.one;
    }

    void SetColliderSize(Vector2 newSize)
    {
        if (hitboxCollider is BoxCollider2D box)
            box.size = newSize;
        else if (hitboxCollider is CircleCollider2D circ)
            circ.radius = Mathf.Max(newSize.x, newSize.y) * 0.5f;
    }

    private void SpawnVFX()
    {
        Vector3 pos = vfxSpawnPoint != null ? vfxSpawnPoint.position : transform.position;
        Quaternion rot = vfxSpawnPoint != null ? vfxSpawnPoint.rotation : Quaternion.identity;
        Instantiate(bloodKnightVFX, pos, rot);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.TryGetComponent<IDamageable>(out var target))
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            target.TakeDamage(damage, transform.position, dir);
        }
    }
}

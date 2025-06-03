using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Collider2D hitbox;
    [Header("Hitbox Offset")]
    [SerializeField] private Vector2 baseOffset = new Vector2(1f, 0f);
    [Header("VFX Prefabs (1~3)")]
    [SerializeField] private GameObject[] hitVFXPrefabs = new GameObject[3];
    [Header("VFX Spawn Point")]
    [SerializeField] private Transform weaponVfxPoint;

    [Header("Combo Settings")]
    [Tooltip("콤보 리셋까지 허용 시간(초)")]
    [SerializeField] private float comboResetTime = 0.6f;
    [Header("Level Gating & Damage Boost")]
    [Tooltip("3타 콤보를 해제할 플레이어 레벨")]
    [SerializeField] private int skillUnlockLevel = 10;
    [Tooltip("언락 레벨 이후 레벨당 데미지 보너스 비율 (예: 0.02 → +2% per level)")]
    [SerializeField] private float damageBoostPerLevel = 0.05f;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private int comboStep = 1;
    private float lastAttackTime;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (hitbox != null)
            hitbox.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            float now = Time.time;
            if (now > lastAttackTime + comboResetTime) comboStep = 1;
            else comboStep = (comboStep % 3) + 1;
            lastAttackTime = now;

            anim.SetTrigger("Attack");
        }
    }

    public void EnableHitBox()
    {
        if (hitbox == null || spriteRenderer == null) return;

        float dir = spriteRenderer.flipX ? -1f : 1f;
        hitbox.transform.localPosition = new Vector2(baseOffset.x * dir, baseOffset.y);
        hitbox.gameObject.SetActive(true);

        int lvl = PlayerLevel.instance.currentLevel;

        if (lvl >= 10)
        {
            int idx = comboStep - 1;
            if (idx < 0 || idx >= hitVFXPrefabs.Length || hitVFXPrefabs[idx] == null)
                return;
            GameObject prefab = hitVFXPrefabs[idx];

            Vector3 basePos = transform.position;
            Vector3 localOffset = weaponVfxPoint != null
                ? weaponVfxPoint.localPosition
                : Vector3.zero;

            Vector3 worldOffset = new Vector3(localOffset.x * dir, localOffset.y, 0f);
            Vector3 spawnPos = basePos + worldOffset;

            spawnPos.z -= 0.1f;

            var go = Instantiate(prefab, spawnPos, Quaternion.identity);

            var spr = go.GetComponent<Renderer>();
            if (spr != null)
            {
                spr.sortingLayerName = spriteRenderer.sortingLayerName;
                spr.sortingOrder = spriteRenderer.sortingOrder + 1;
            }

            if (spriteRenderer.flipX)
            {
                var spriteR = go.GetComponent<SpriteRenderer>();
                if (spriteR != null)
                    spriteR.flipX = true;
            }

            Destroy(go, 0.5f);
        }
        }
    
    public void DisableHitBox()
    {
        if (hitbox != null)
            hitbox.gameObject.SetActive(false);
    }

    public void OnAttackAnimEnd()
    {
        DisableHitBox();
    }
}

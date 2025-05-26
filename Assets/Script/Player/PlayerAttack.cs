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
            // 리셋 타임 지나면 1타부터, 아니면 순환 (1→2→3→1)
            if (now > lastAttackTime + comboResetTime) comboStep = 1;
            else comboStep = (comboStep % 3) + 1;
            lastAttackTime = now;

            anim.SetTrigger("Attack");
        }
    }

    // 애니메이션 이벤트에 연결 ▶ 타격 판정 + VFX
    public void EnableHitBox()
    {
        if (hitbox == null || spriteRenderer == null) return;

        // 1) 히트박스 위치 조정 & 활성화
        float dir = spriteRenderer.flipX ? -1f : 1f;
        hitbox.transform.localPosition = new Vector2(baseOffset.x * dir, baseOffset.y);
        hitbox.gameObject.SetActive(true);

        // 2) 해당 콤보 단계 VFX 스폰
        int idx = comboStep - 1;
        if (idx < 0 || idx >= hitVFXPrefabs.Length || hitVFXPrefabs[idx] == null)
            return;
        GameObject prefab = hitVFXPrefabs[idx];

        // 2) 좌우 반전 보정된 위치 계산
        Vector3 basePos = transform.position;
        Vector3 localOffset = weaponVfxPoint != null
            ? weaponVfxPoint.localPosition
            : Vector3.zero;
        // flipX일 때 X만 반전
        Vector3 worldOffset = new Vector3(localOffset.x * dir, localOffset.y, 0f);
        Vector3 spawnPos = basePos + worldOffset;

        // 3) Z 오프셋으로 카메라 쪽으로 살짝 당기기
        spawnPos.z -= 0.1f;

        // 4) VFX 인스턴스화
        var go = Instantiate(prefab, spawnPos, Quaternion.identity);

        // 5) 스프라이트 렌더러 정렬 순서 올리기
        var spr= go.GetComponent<Renderer>();
        if (spr != null)
        {
            spr.sortingLayerName = spriteRenderer.sortingLayerName;
            spr.sortingOrder = spriteRenderer.sortingOrder + 1;
        }

        // 6) 좌우 반전된 스프라이트가 필요하다면
        if (spriteRenderer.flipX)
        {
            var spriteR = go.GetComponent<SpriteRenderer>();
            if (spriteR != null)
                spriteR.flipX = true;
        }

        Destroy(go, 0.5f);
    
        }
    
    // 애니메이션 이벤트에 연결 ▶ 히트박스 끄기
    public void DisableHitBox()
    {
        if (hitbox != null)
            hitbox.gameObject.SetActive(false);
    }

    // 애니메이션 끝 프레임 이벤트로 연결 ▶ 히트박스 보장 비활성화
    public void OnAttackAnimEnd()
    {
        DisableHitBox();
    }
}

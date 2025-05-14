using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Collider2D hitbox;
    [Header("Hitbox Offset")]
    [SerializeField] Vector2 baseOffset = new Vector2(1f, 0f);
    /// <summary>
    /// 
    /// </summary>
    Animator anim;
    SpriteRenderer spriteRenderer;
    /// <summary>
    /// 
    /// </summary>
    static readonly int HashAttack = Animator.StringToHash("Attack");
    static readonly int HashAttackCount = Animator.StringToHash("AtkCount");

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (hitbox) hitbox.gameObject.SetActive(false);   // 기본 OFF
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            int next = (anim.GetInteger(HashAttackCount) + 1) % 3;
            anim.SetInteger(HashAttackCount, next);
            anim.SetTrigger(HashAttack);
        }
    }

    /* ───── Animation Event 콜백 ───── */
    public void EnableHitBox()
    {
        if (hitbox == null || spriteRenderer == null) return;
        // 1) 바라보는 방향에 맞춰 오프셋 반전
        float dir = spriteRenderer.flipX ? -1f : 1f;
        hitbox.transform.localPosition = new Vector2(baseOffset.x* dir, baseOffset.y);
       // 2) 히트박스 활성화
       hitbox.gameObject.SetActive(true);
    }
    public void DisableHitBox()
    {
        if (hitbox == null) return;
        hitbox.gameObject.SetActive(false);
    }

// 필요하다면 공격 애니 끝 프레임에 붙여서 콤보 초기화
public void OnAttackAnimEnd()
    {
        anim.SetInteger(HashAttackCount, 0);
        DisableHitBox();
    }
}


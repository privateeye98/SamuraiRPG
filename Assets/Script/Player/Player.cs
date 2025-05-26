using UnityEngine;
using System.Collections;
using System;
using UnityEditor.Rendering.LookDev;
public class Player : MonoBehaviour, IDamageable
{
    // -- components -- 
    Rigidbody2D rb;
    public float maxSpeed;
    public float animAccel = 5f;
    SpriteRenderer spriteRenderer;
    Animator anim;



    public float jumpForce;
    public LayerMask groundMask;
    public Vector2 groundCheckSize = new Vector2(0.45f, 0.05f);
    bool isGrounded;

    //-- Dash related
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    float lastDashTime = 0f;
    float dashTime = 0f;
    bool isDashing = false;
    Vector2 dashDirection;

    // -- 잔상
    public GameObject afterImagePrefab;
    public float afterImageInterval = 0.05f;
    float afterImageTimer = 0f;

    //-- SKILL
    [SerializeField] Transform skillSpawnPoint;
    [SerializeField] GameObject skillHitboxPrefab;
    [SerializeField] int specialSkillCost = 70;
    [SerializeField] string specialTriggerName = "Special"; // animator Trigger
    [SerializeField] float SkillCooldown = 2f;
    [SerializeField] float skillAfterImageDuration = 0.5f;
    [SerializeField] float skillAfterImageInterval = 0.05f;
    float lastSkillTime = -999f;

    [Header("Wide Skill Hitbox (광역)")]
    [SerializeField] int wideSkillCost = 500;
    [SerializeField] GameObject skillPortraitUI;
    [SerializeField] float wideSkillCooldown = 5f;
    float lastWideSkillTime = -999f;
    [SerializeField] float wideSkillMultiplier = 1.8f; // 퍼뎀 (180%)
    [SerializeField] int wideSkillBaseDamage = 10;     // 기본값

    [Header("Wide Skill VFX")]
    [SerializeField] private GameObject wideSkillVfxPrefab;    // 광역 스킬용 VFX 프리팹
    [SerializeField] private Transform wideSkillVfxPoint;      // 이펙트가 나올 위치(예: 플레이어 중심)
    [SerializeField] private float wideSkillVfxLifetime = 1f;  // 자동 삭제 시간

    public static Player instance;
    void SpawnSkillHitbox()
    {
        if (skillHitboxPrefab && skillSpawnPoint)
        {
            Instantiate(skillHitboxPrefab, skillSpawnPoint.position, Quaternion.identity);
        }
    }

    IEnumerator StartSkillAfterImages()
    {
        float time = 0f;
        while (time < skillAfterImageDuration)
        {
            CreateAfterImage(); // 기존에 만든 메서드 재사용
            time += skillAfterImageInterval;
            yield return new WaitForSeconds(skillAfterImageInterval);
        }
    }
    void Start()
    {
        StartCoroutine(AssignCameraTargetAfterDelay());
    }

    IEnumerator AssignCameraTargetAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // 카메라가 먼저 Awake되도록 기다림

        var cam = FindObjectOfType<TMPro.Examples.CameraController>();
        if (cam != null)
        {
            cam.CameraTarget = this.transform;
            Debug.Log("📷 CameraTarget 연결 완료");
        }
    }

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }


    void Update()
    {

        // -- Jump
        if (Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("isJumping"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        // -- stop speed
        if (Input.GetButtonUp("Horizontal"))
            rb.linearVelocity = new Vector2(rb.linearVelocity.normalized.x * 0.5f, rb.linearVelocity.y);

        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //-- Idle animator
        if (Mathf.Abs(rb.linearVelocity.x) < 0.3f)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)           // 추가
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);     // 추가

        //-- Dash animator
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time > lastDashTime + dashCooldown)
        {
            isDashing = true;
            dashTime = dashDuration;
            lastDashTime = Time.time;

            float direction = Input.GetAxisRaw("Horizontal");
            if (direction == 0)
            {
                direction = spriteRenderer.flipX ? -1 : 1;
            }

            dashDirection = new Vector2(direction, 0).normalized;



            rb.linearVelocity = Vector2.zero; 
            rb.position += dashDirection * dashSpeed * Time.fixedDeltaTime * 8f; 

            anim.SetBool("isDashing", true);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool enoughMana = PlayerStat.instance != null &&
                      PlayerStat.instance.currentMP >= specialSkillCost;
            bool cooldownReady = Time.time >= lastSkillTime + SkillCooldown;

            if (enoughMana && cooldownReady)
            {

                PlayerStat.instance.UseMana(specialSkillCost);
                anim.SetTrigger(specialTriggerName);
                lastSkillTime = Time.time; // 쿨타임 기록


                StartCoroutine(StartSkillAfterImages());
                rb.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);

            }
            else if (!cooldownReady)
            {
                Debug.Log("스킬 쿨타임 진행 중...");
            }
            else
            {
                Debug.Log("마나 부족!");
            }

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            bool enoughMana = PlayerStat.instance != null &&
                  PlayerStat.instance.currentMP >= specialSkillCost;
            bool cooldownReady = Time.time >= lastWideSkillTime + wideSkillCooldown;

            if (enoughMana && cooldownReady)
            {

                PlayerStat.instance.UseMana(wideSkillCost);
                lastWideSkillTime = Time.time;


                CameraShake.instance?.StartCoroutine(CameraShake.instance.Shake(0.3f, 0.2f));
                float dir = GetComponent<SpriteRenderer>().flipX ? -1f : 1f;

                DamageAllEnemies(); //
                if (wideSkillVfxPrefab != null)
                {
                    Vector3 spawnPos = (wideSkillVfxPoint != null)
                         ? wideSkillVfxPoint.position : transform.position;

                    spawnPos.y += 3f;
                    spawnPos.x += dir * 1f;
                    var go = Instantiate(wideSkillVfxPrefab, spawnPos, Quaternion.identity);
                    Destroy(go, wideSkillVfxLifetime);

                    var s = go.transform.localScale;
                    go.transform.localScale = new Vector3(Mathf.Abs(s.x) * dir, s.y, s.z);
                }

            }
            else if (!cooldownReady)
            {
                Debug.Log("광역 스킬 쿨타임 진행 중...");
            }
            else
            {
                Debug.Log("마나 부족!");
            }
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            PlayerStat.instance.RecoverMana(100);
            Debug.Log("🔵 마나 +100 회복됨");
        }
    }


    public void TakeDamage(int dmg, Vector2 hitpoint, Vector2 hitDir)
    {
        PlayerHealth.instance.TakeDamage(dmg);
        Debug.Log("플레이어가 공격당함!");
    }
    void FixedUpdate()
    {


        // Dashing
        if (isDashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;

            //--create after image;
            afterImageTimer -= Time.fixedDeltaTime;
            if (afterImageTimer <= 0f)
            {
                CreateAfterImage();
                afterImageTimer = afterImageInterval;
            }
            dashTime -= Time.fixedDeltaTime;
            // -- dash time
            if (dashTime <= 0)
            {
                isDashing = false;
                anim.SetBool("isDashing", false);
            }
            return;
        }
        // Movement
        float h = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(h * maxSpeed, rb.linearVelocity.y);
        if (Mathf.Abs(h) > 0.01f)
            spriteRenderer.flipX = h < 0;

        rb.AddForce(Vector2.right * h * 10f, ForceMode2D.Force);


        // Clamp the player's speed
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed), rb.linearVelocity.y);   // �� ����

        float targetSpeed = Mathf.Abs(rb.linearVelocity.x) / maxSpeed;  // 0~1
        targetSpeed = Mathf.Lerp(1f, 2f, targetSpeed);

        anim.speed = Mathf.Lerp(anim.speed, targetSpeed,
                                Time.fixedDeltaTime * animAccel);


        if (rb.linearVelocity.y < 0)
        {
            Debug.DrawRay(transform.position, Vector3.down * 2f, Color.blue);
            // Check if the player is grounded
            RaycastHit2D rH = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rH.collider != null)
            {
                Debug.Log(rH.collider.name);
                anim.SetBool("isJumping", false);
            }
        }



    }

    void CreateAfterImage(bool isSkill = false)
    {
        if (afterImagePrefab == null) return;

        GameObject img = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);
        SpriteRenderer imgSpr = img.GetComponent<SpriteRenderer>();
        SpriteRenderer playerSpr = spriteRenderer;

        if (imgSpr != null && playerSpr != null)
        {
            imgSpr.sprite = playerSpr.sprite;
            imgSpr.flipX = playerSpr.flipX;
            imgSpr.transform.localScale = transform.localScale;
            imgSpr.color = new Color(1f, 1f, 1f, 0.5f); // 50% alpha
            if (isSkill)
                imgSpr.color = new Color(0.5f, 1f, 1f, 0.6f);  // 청록색 스킬용
            else
                imgSpr.color = new Color(1f, 1f, 1f, 0.5f);

        }
    }


    void DamageAllEnemies()
    {
        float str = PlayerStat.instance.strength;
        float dex = PlayerStat.instance.dexterity;

        float rawDamage = wideSkillBaseDamage + (str * 2f) + (dex * 0.5f);
        int totalDamage = Mathf.RoundToInt(rawDamage * wideSkillMultiplier);

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemyObj in enemies)
        {
            if (enemyObj.TryGetComponent<IDamageable>(out var target))
            {
                Vector2 hitDir = enemyObj.transform.position - transform.position;
                target.TakeDamage(totalDamage, enemyObj.transform.position, hitDir.normalized);
            }
        }

        Debug.Log($"전체 적 {enemies.Length}명에게 {totalDamage} 데미지 (퍼뎀 {wideSkillMultiplier}배)");
    }



}
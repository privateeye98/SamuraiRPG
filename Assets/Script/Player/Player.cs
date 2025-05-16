using UnityEngine;
using System.Collections;
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

    void Awake()
    {
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



            rb.linearVelocity = Vector2.zero; // Reset velocity before dashing
            rb.position += dashDirection * dashSpeed * Time.fixedDeltaTime * 8f; // Apply dash speed

            anim.SetBool("isDashing", true);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool enoughMana = PlayerMana.instance != null &&
                      PlayerMana.instance.currentMP >= specialSkillCost;
            bool cooldownReady = Time.time >= lastSkillTime + SkillCooldown;

            if (enoughMana && cooldownReady)
            {
                PlayerMana.instance.UseMana(specialSkillCost);
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
    }


    public void TakeDamage(int dmg,Vector2 hitpoint,Vector2 hitDir)
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
            if(afterImageTimer <= 0f)
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
            Debug.DrawRay(transform.position, Vector3.down * 1f, Color.blue);
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

        if(imgSpr != null && playerSpr != null)
        {
            imgSpr.sprite = playerSpr.sprite;
            imgSpr.flipX = playerSpr.flipX;
            imgSpr.transform.localScale = transform.localScale;
            imgSpr.color = new Color(1f,1f,1f, 0.5f); // 50% alpha
            if (isSkill)
                imgSpr.color = new Color(0.5f, 1f, 1f, 0.6f);  // 청록색 스킬용
            else
                imgSpr.color = new Color(1f, 1f, 1f, 0.5f);

        }
    }

}
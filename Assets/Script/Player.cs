using UnityEngine;

public class Player : MonoBehaviour
{
    // -- components -- 
    Rigidbody2D rb;
    public float maxSpeed;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public float animAccel = 5f;


    public float jumpForce;
    public LayerMask groundMask;
    public Vector2 groundCheckSize = new Vector2(0.45f, 0.05f);
    bool isGrounded;


    //attack
    [SerializeField] Collider2D attackCol;


    //combo reference
    int comboStep = 0;
    float comboTimer = 0f;
    [SerializeField] float comboTime = 0.4f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // -- start false --
        if (attackCol) attackCol.enabled = false;
    }


    void Update()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("isJumping"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        // stop speed
        if (Input.GetButtonUp("Horizontal"))
            rb.linearVelocity = new Vector2(rb.linearVelocity.normalized.x * 0.5f, rb.linearVelocity.y);

        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // animator
        if (Mathf.Abs(rb.linearVelocity.x) < 0.3f)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)           // 추가
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);     // 추가


        // --  Attack

        if (Time.time > comboTimer)
            comboStep = 0;


        if (Input.GetKeyDown(KeyCode.Z))
        {

            anim.ResetTrigger("AttackTrigger");
            anim.SetFloat("AttackIndex", comboStep);
            anim.SetTrigger("AttackTrigger");

            comboStep = (comboStep + 1) % 3;
            comboTimer = Time.time + comboTime;

        }


    }



    void FixedUpdate()
    {
        // �̵�
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


    // -- HitBox --
    public void EnableHitBox()
    {
        if (attackCol != null)
            attackCol.enabled = true;
    }
    public void DisableHitBox()
    {
        if (attackCol != null)
            attackCol.enabled = false;
    }

  

}
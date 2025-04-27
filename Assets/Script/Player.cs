using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float maxSpeed;

     void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {   //Jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
  
            rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
    }
    void FixedUpdate()
    {
        // Player Movement
        float h = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Vector2.right * h, ForceMode2D.Impulse);


        // Clamp the player's speed
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed), rb.linearVelocity.y);
    }
}

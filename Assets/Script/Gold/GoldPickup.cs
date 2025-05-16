using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int goldAmount = 10;

     void Start()
    {
        Collider2D goldCol = GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && goldCol != null)
        {
            Collider2D playerCol = player.GetComponent<Collider2D>();
            if (playerCol != null)
                Physics2D.IgnoreCollision(goldCol, playerCol);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("¡¢√À: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("∞ÒµÂ »πµÊ!");
            GoldManager.instance.AddGold(goldAmount);
            Destroy(gameObject);
        }
    }
}

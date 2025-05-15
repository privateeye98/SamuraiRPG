using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    public ItemData potion;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger 충돌 감지됨: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌 확인됨");

            if (Inventory.instance.AddItem(potion))
            {
                Debug.Log($"{potion.itemName} 획득!");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("포션 추가 실패!");
            }
        }
    }
}
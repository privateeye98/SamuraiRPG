using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    public ItemData potion;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger �浹 ������: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� �浹 Ȯ�ε�");

            if (Inventory.instance.AddItem(potion))
            {
                Debug.Log($"{potion.itemName} ȹ��!");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("���� �߰� ����!");
            }
        }
    }
}
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public int maxHP = 100;
    public int currentHP;

     void Awake()
    {
        instance = this;
        currentHP = maxHP;
    }
    public void Heal(int amount)
    {
        currentHP =Mathf.Min(currentHP + amount, maxHP);
        Debug.Log("Healed: " + amount + ", Current HP: " + currentHP);
    }

    public void TakeDamage(int dmg)
    {
        currentHP = Mathf.Max(currentHP - dmg, 0);
        Debug.Log($"���ع��� : {dmg} / ���� ü�� :{currentHP}");
    }
}

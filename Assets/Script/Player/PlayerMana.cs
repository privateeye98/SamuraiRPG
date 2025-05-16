using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    public static PlayerMana instance;

    public int maxMP = 100;
    public int currentMP;

    void Awake()
    {
        instance = this;
        currentMP = maxMP;

    }

    public void UseMana(int amount)
    {
        if (currentMP >= amount)
        {
            currentMP -= amount;
            Debug.Log($"마나 소모: {amount}, 현재 MP: {currentMP}");
        }
        else
        {
            Debug.Log("마나부족!");
        }
    }
    public void RecoverMana(int amount)
    {
        currentMP = Mathf.Min(currentMP + amount, maxMP);
        Debug.Log($"마나 회복: {amount}, 현재 MP: {currentMP}");
    }

}

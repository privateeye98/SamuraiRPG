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
            Debug.Log($"���� �Ҹ�: {amount}, ���� MP: {currentMP}");
        }
        else
        {
            Debug.Log("��������!");
        }
    }
    public void RecoverMana(int amount)
    {
        currentMP = Mathf.Min(currentMP + amount, maxMP);
        Debug.Log($"���� ȸ��: {amount}, ���� MP: {currentMP}");
    }

}

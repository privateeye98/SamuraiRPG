using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;
    public int currentGold = 0;
    
     void Awake()
    { 
        if(instance != null && instance !=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
          }

    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log($"��� ȹ��: +{amount}, �� ���: {currentGold}");
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            Debug.Log($"��� ���: -{amount}, ���� ���: {currentGold}");
            return true;
        }
        Debug.Log("��� ����!");
        return false;
    }

    public void SetGold(int amount)
    {
        currentGold = amount;
    }


}

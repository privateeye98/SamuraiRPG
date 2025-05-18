using UnityEngine;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<ItemData> items = new List<ItemData>();
    public int capacity = 9;

    public delegate void OnItemChanged();
    public event OnItemChanged onItemChangedCallback;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool AddItem(ItemData item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Not enough room.");
            return false;
        }

        items.Add(item);
        Debug.Log($"{item.name} 아이템 추가됨");
        onItemChangedCallback?.Invoke(); // UI 갱신 호출


        return true;
    }


    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        onItemChangedCallback?.Invoke();
    }

    public void UseItem(ItemData item)
    {
        if (!items.Contains(item)) return;

        Debug.Log("포션 사용됨: " + item.itemName);

        switch (item.type)
        {
            case ItemType.Consumable:
                // 체력이 가득 차 있으면 사용 불가
                if (PlayerHealth.instance.currentHP >= PlayerHealth.instance.maxHP)
                {
                    Debug.Log("체력이 가득 차 있어 아이템을 사용할 수 없습니다.");
                    return;
                }

                PlayerHealth.instance.Heal(item.healAmount);
                RemoveItem(item);
                break;

            case ItemType.Equipment:
                Debug.Log("장비 사용은 아직 구현되지 않음");
                break;

            case ItemType.Quest:
                Debug.Log("퀘스트 아이템은 사용할 수 없습니다.");
                break;
        }

        // 더 이상 자동 판매 안 함
    }
    public void SellItem(ItemData item)
    {
        if (!items.Contains(item)) return;

        GoldManager.instance.AddGold(item.price);
        RemoveItem(item);
        Debug.Log($"{item.itemName}을(를) {item.price}골드에 판매했습니다.");
    }

}

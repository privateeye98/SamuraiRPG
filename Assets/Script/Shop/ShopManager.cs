using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;


    [SerializeField] GameObject shopUI;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Transform itemSlotParent;
    [SerializeField] GameObject itemSlotPrefab;

    public List<ItemData> shopItems = new List<ItemData>();
    void Awake()
    {
        instance = this;
    }


    public void OpenShop()
    {
        shopUI.SetActive(true);
        UpdateUI();
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
    }

    public void BuyItem(ItemData item)
    {
        Debug.Log($"[시도] {item.itemName} 구매 시도");
        Debug.Log($"[현재골드] {GoldManager.instance.currentGold}, [가격] {item.price}");

        if (GoldManager.instance.currentGold < item.price)
        {
            Debug.LogWarning("[실패] 골드 부족");
            return;
        }

        bool success = GoldManager.instance.SpendGold(item.price);
        Debug.Log($"[SpendGold 결과] {success}");

        if (!success)
        {
            Debug.LogError("SpendGold 실패 발생");
            return;
        }

        bool added = Inventory.instance.AddItem(item);
        Debug.Log($"[인벤토리 추가 결과] {added}");

        if (!added)
        {
            Debug.LogWarning("인벤토리에 추가 실패");
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (goldText == null || itemSlotParent == null || itemSlotPrefab == null)
        {
            Debug.LogError("UpdateUI - 필드 연결 누락!");
            return;
        }
        if (goldText == null)
        {
            Debug.LogError("goldText가 null입니다! 인스펙터에서 연결 확인 필요");
            return;
        }

        if (GoldManager.instance == null)
        {
            Debug.LogError("GoldManager.instance가 null입니다! GoldManager 오브젝트가 씬에 있는지 확인");
            return;
        }

        goldText.text = "Gold: " + GoldManager.instance.currentGold;

        foreach (Transform child in itemSlotParent)
            Destroy(child.gameObject);

        foreach (var item in shopItems)
        {
            if (item == null)
            {
                Debug.LogError("ShopItems 안에 null 아이템 있음!");
                continue;
            }

            Debug.Log($"[SHOP] 슬롯 생성 시도: {item.itemName}");
            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);

            var shopSlot = slot.GetComponent<ShopItemSlot>();
            if (shopSlot == null)
            {
                Debug.LogError("ShopItemSlot 컴포넌트가 프리팹에 없음!");
                return;
            }

            shopSlot.Setup(item);
        }
    }

}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] private Vector2 inventoryShopPosition = new Vector2(600f, 0f);
    [Header("UI References")]
    [SerializeField] RectTransform inventoryUI;
    [SerializeField] GameObject shopUI;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Transform itemSlotParent;
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] RectTransform inventoryUI_Shop;
    [SerializeField] GameObject buyPopup;
    [SerializeField] TMP_InputField amountInput;
    ItemData selectedItem;

    public List<ItemData> shopItems = new List<ItemData>();
    public GameObject shopInventoryUI;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (shopUI != null && shopUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }
    public void OpenShop()
    {
        CloseNPCUI();
        Debug.Log("OpenShop 호출됨");
        shopUI.SetActive(true);
        if (inventoryUI_Shop != null)
        {
            CloseNPCUI(); // Dialogue 닫기

            shopUI.SetActive(true);

            if (shopInventoryUI != null)
                shopInventoryUI.SetActive(true);

            Debug.Log("🛒 상점 + 인벤토리 열림");
        }
        UpdateUI();
    }
    public void CloseShop()
    {
        shopUI.SetActive(false);
        if (inventoryUI_Shop != null)
            inventoryUI_Shop.gameObject.SetActive(false);

        if (DialogueManager.instance != null)
            DialogueManager.instance.ForceCloseDialogue();

        InventoryToggle toggle = FindObjectOfType<InventoryToggle>();
        if (toggle != null)
            toggle.isShopOpen = false;
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

    void PopulateShop()
    {
        foreach (Transform child in itemSlotParent)
            Destroy(child.gameObject);

        foreach (var item in shopItems)
        {
            if (item == null) continue;

            GameObject slot = Instantiate(itemSlotPrefab, itemSlotParent);
            ShopItemSlot shopSlot = slot.GetComponent<ShopItemSlot>();
            if (shopSlot != null)
                shopSlot.Setup(item);
        }
    }
    public void CloseNPCUI()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.ForceCloseDialogue();
            Debug.Log("📦 [ShopManager] DialoguePanel 닫음");
        }


    }
    public void OpenBuyAmountPopup(ItemData item)
    {
        selectedItem = item;
        buyPopup.SetActive(true);
        amountInput.text = "1";
    }
    public void ConfirmBuy()
    {
        if (selectedItem == null) return;

        if (!int.TryParse(amountInput.text, out int amount) || amount <= 0)
        {
            Debug.Log("잘못된 수량 입력!");
            return;
        }
        int totalCost = selectedItem.price * amount;

        if (GoldManager.instance.SpendGold(totalCost))
        {
            for (int i = 0; i < amount; i++)
            {
                Inventory.instance.AddItem(selectedItem);
            }
            Debug.Log($"{selectedItem.itemName}을(를) {amount}개 구매했습니다.");
        }
        else
        {
            Debug.Log("골드 부족!");
        }

        buyPopup.SetActive(false);
    }
    public void CancelBuy()
    {
        selectedItem = null;
        buyPopup.SetActive(false);
    }

}

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

    [Header("UI References (판매)")]
    [SerializeField] private GameObject sellUI;        
    [SerializeField] private Transform sellSlotParent;     
    [SerializeField] private GameObject sellSlotPrefab;
    public List<ItemData> shopItems = new List<ItemData>();
    public GameObject shopInventoryUI;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 기존 싱글톤 제거
            return;
        }
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
        sellUI.SetActive(false);
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
        if (!int.TryParse(amountInput.text, out int amount) || amount <= 0)
            return;

        int totalCost = selectedItem.price * amount;
        if (!GoldManager.instance.SpendGold(totalCost))
        {
            Debug.Log("골드가 부족합니다!");
            return;
        }
        bool added = Inventory.instance.AddItem(selectedItem, amount);
        if (!added)
            Debug.LogWarning("인벤토리에 공간이 부족합니다!");
        else
            Debug.Log($"{selectedItem.itemName}을(를) {amount}개 구매했습니다.");

        buyPopup.SetActive(false);

        UpdateUI();
    }

public void CancelBuy()
    {
        selectedItem = null;
        buyPopup.SetActive(false);
    }
    private void UpdateGoldDisplay()
    {
        if (goldText != null && GoldManager.instance != null)
            goldText.text = "Gold: " + GoldManager.instance.currentGold;
    }
    public void OpenBuyTab()
    {
        // 1) 판매 UI는 숨기고
        sellUI.SetActive(false);
        // 2) 구매 UI를 띄우고
        //    (purchase slots는 shopUI 하위라고 가정)
        if (itemSlotParent != null && itemSlotPrefab != null)
        {
            foreach (Transform child in itemSlotParent) Destroy(child.gameObject);
            foreach (var item in shopItems)
            {
                if (item == null) continue;
                GameObject slotGO = Instantiate(itemSlotPrefab, itemSlotParent);
                slotGO.GetComponent<ShopItemSlot>().Setup(item);
            }
        }
        UpdateGoldDisplay();
    }


    // ────────────────────────────────────────────────────────────────────
    // “판매 탭” 관련
    // ────────────────────────────────────────────────────────────────────
    public void OpenSellTab()
    {
        foreach (Transform child in itemSlotParent)
            Destroy(child.gameObject);

        sellUI.SetActive(true);

        foreach (Transform child in sellSlotParent)
            Destroy(child.gameObject);

        foreach (var invItem in Inventory.instance.items)
        {
            if (CanSell(invItem.itemData))
            {
                GameObject slotGO = Instantiate(sellSlotPrefab, sellSlotParent);
                slotGO.GetComponent<SellItemSlot>().Setup(invItem);
            }
        }

        UpdateGoldDisplay();
    }

    private bool CanSell(ItemData data)
    {
        // 예시: 퀘스트 아이템은 판매 불가
        if (data.type == ItemType.Quest) return false;
        return true;
    }



}

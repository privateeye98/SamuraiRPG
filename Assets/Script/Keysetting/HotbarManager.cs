using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager instance;

    public HotbarSlot[] slots;


    [SerializeField] private Transform hotbarParent;
    [SerializeField] private GameObject hotbarSlotPrefab;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        InitializeDefaults();
    }
    private void Start()
    {
        CreateUI();
    }


    private void InitializeDefaults()
    {
        //2칸세팅
        if (slots == null || slots.Length != 0)
        {
            slots = new HotbarSlot[2];
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) slots[i] = new HotbarSlot();
            slots[i].assignedKey = KeyCode.Alpha1 + i;

        }
    }

    private void CreateUI()
    {
        if (hotbarParent == null || hotbarSlotPrefab == null)
        {
            Debug.LogError("[HotbarManager] hotbarParent 또는 hotbarSlotPrefab이 할당되지 않았습니다.");
            return;
        }

        // 1) 부모에 남아 있는 기존 슬롯들 모두 삭제
        for (int i = hotbarParent.childCount - 1; i >= 0; i--)
        {
            Destroy(hotbarParent.GetChild(i).gameObject);
        }

        // 2) 슬롯 간 간격 및 크기 설정 (필요시 Inspector에 노출해도 좋음)
        float slotWidth = 50f;   // Prefab에서 설정한 SizeDelta.x
        float slotHeight = 50f;   // Prefab에서 설정한 SizeDelta.y
        float spacing = 100f;  // 슬롯들 사이 간격
        float paddingLeft = 10f;   // 왼쪽 여백
        float paddingTop = 5f;    // 위쪽 여백

        for (int i = 0; i < slots.Length; i++)
        {
            GameObject go = Instantiate(hotbarSlotPrefab, hotbarParent);
            var slotUI = go.GetComponent<HotbarSlotUI>();
            if (slotUI != null)
            {
                slotUI.Initialize(i); // 슬롯 Index를 알려 줌
                // 예: slotUI.RefreshUI(); 같은 초기화 로직
            }

            // 3) Instantiate 후, RectTransform 포인트 설정
            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt == null)
            {
                Debug.LogError("[HotbarManager] HotbarSlotPrefab에 RectTransform이 없습니다!");
                continue;
            }

            // Anchor 및 Pivot을 “왼쪽 상단”으로 고정
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);

            // i번째 슬롯의 x, y 좌표 계산
            float posX = paddingLeft + i * (slotWidth + spacing);
            float posY = -paddingTop;
            // ※ 주의: rt.anchor가 (0,1) 상태에서
            //    anchoredPosition.y = 0 이면 “부모 위쪽(Top)”과 딱 맞닿습니다.
            //    위쪽에서 5px 아래로 띄우고 싶다면 
            //    anchoredPosition.y = -5 로 설정해야 합니다.

            rt.anchoredPosition = new Vector2(posX, posY);

            // 슬롯 크기는 Prefab에서 이미 SizeDelta=(50,50)으로 고정했으므로
            // 만약 런타임에 사이즈를 조정하고 싶으면 이 줄을 사용:
            // rt.sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }

    private void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            KeyCode kode = slots[i].assignedKey;
            if (kode != KeyCode.None && Input.GetKeyDown(kode))
            {
                var data = slots[i].assignedItemData;
                if (data != null)
                {
                    Inventory.instance.UseItem(data);
                    RefreshAllSlots();
                }
            }
        }
    }
    public void RefreshAllSlots()
    {
        foreach (Transform child in hotbarParent)
        {
            var slotUI = child.GetComponent<HotbarSlotUI>();
            if (slotUI != null)
                slotUI.RefreshUI();
        }
    }

}

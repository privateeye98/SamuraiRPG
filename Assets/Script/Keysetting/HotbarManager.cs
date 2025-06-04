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
        //2ĭ����
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
            Debug.LogError("[HotbarManager] hotbarParent �Ǵ� hotbarSlotPrefab�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // 1) �θ� ���� �ִ� ���� ���Ե� ��� ����
        for (int i = hotbarParent.childCount - 1; i >= 0; i--)
        {
            Destroy(hotbarParent.GetChild(i).gameObject);
        }

        // 2) ���� �� ���� �� ũ�� ���� (�ʿ�� Inspector�� �����ص� ����)
        float slotWidth = 50f;   // Prefab���� ������ SizeDelta.x
        float slotHeight = 50f;   // Prefab���� ������ SizeDelta.y
        float spacing = 100f;  // ���Ե� ���� ����
        float paddingLeft = 10f;   // ���� ����
        float paddingTop = 5f;    // ���� ����

        for (int i = 0; i < slots.Length; i++)
        {
            GameObject go = Instantiate(hotbarSlotPrefab, hotbarParent);
            var slotUI = go.GetComponent<HotbarSlotUI>();
            if (slotUI != null)
            {
                slotUI.Initialize(i); // ���� Index�� �˷� ��
                // ��: slotUI.RefreshUI(); ���� �ʱ�ȭ ����
            }

            // 3) Instantiate ��, RectTransform ����Ʈ ����
            RectTransform rt = go.GetComponent<RectTransform>();
            if (rt == null)
            {
                Debug.LogError("[HotbarManager] HotbarSlotPrefab�� RectTransform�� �����ϴ�!");
                continue;
            }

            // Anchor �� Pivot�� ������ ��ܡ����� ����
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);

            // i��° ������ x, y ��ǥ ���
            float posX = paddingLeft + i * (slotWidth + spacing);
            float posY = -paddingTop;
            // �� ����: rt.anchor�� (0,1) ���¿���
            //    anchoredPosition.y = 0 �̸� ���θ� ����(Top)���� �� �´���ϴ�.
            //    ���ʿ��� 5px �Ʒ��� ���� �ʹٸ� 
            //    anchoredPosition.y = -5 �� �����ؾ� �մϴ�.

            rt.anchoredPosition = new Vector2(posX, posY);

            // ���� ũ��� Prefab���� �̹� SizeDelta=(50,50)���� ���������Ƿ�
            // ���� ��Ÿ�ӿ� ����� �����ϰ� ������ �� ���� ���:
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

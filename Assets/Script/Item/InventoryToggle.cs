using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;
    public Vector2 centerPosition = Vector2.zero;
    public Vector2 shopOpenPosition = new Vector2(400f, 0f);
    public bool isShopOpen = false;
    void Awake()
    {
        if (inventoryUI.transform.parent != null)
            inventoryUI.transform.SetParent(null);
        DontDestroyOnLoad(inventoryUI);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI != null)
            {
                bool nowActive = !inventoryUI.activeSelf;
                inventoryUI.SetActive(nowActive);

                if (nowActive && !isShopOpen)
                {
                    inventoryUI.transform.SetParent(GameObject.Find("Canvas").transform);

                    RectTransform rt = inventoryUI.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = Vector2.zero;
                }
            }
        }
    }

    public void ToggleInventory()
    {
        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive);

        if (!isActive)
        {
            inventoryUI.GetComponent<RectTransform>().anchoredPosition = centerPosition;
        }
    }
}

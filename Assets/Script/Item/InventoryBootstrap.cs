using UnityEngine;

public class InventoryBootstrap : MonoBehaviour
{
    [SerializeField] GameObject inventoryPrefab;

    void Awake()
    {
        if (Inventory.instance == null)
        {
            GameObject go = Instantiate(inventoryPrefab);
            go.name = "InventoryManager";
            DontDestroyOnLoad(go);
        }
    }
}

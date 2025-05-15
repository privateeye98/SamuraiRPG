using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI != null)
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
            }
        }
    }
}
using UnityEngine;

public class EquipmentPanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject equipmentPanel; 
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equipmentPanel == null)
            {
                Debug.LogWarning("EquipmentPanelToggle: equipmentPanel 오브젝트가 할당되지 않았습니다!");
                return;
            }

            isOpen = !isOpen;
            equipmentPanel.SetActive(isOpen);
        }
    }
}

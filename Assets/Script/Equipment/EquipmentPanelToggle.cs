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
                Debug.LogWarning("EquipmentPanelToggle: equipmentPanel ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
                return;
            }

            isOpen = !isOpen;
            equipmentPanel.SetActive(isOpen);
        }
    }
}

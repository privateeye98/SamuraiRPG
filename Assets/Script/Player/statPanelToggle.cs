using UnityEngine;

public class StatPanelToggle : MonoBehaviour
{
    [SerializeField] GameObject statPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (statPanel != null)
            {
                bool isActive = statPanel.activeSelf;
                statPanel.SetActive(!isActive);
            }
        }
    }
}

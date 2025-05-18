using UnityEngine;

public class EscapeUIManager : MonoBehaviour
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject shopInventory;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool closed = false;

            if (dialoguePanel != null && dialoguePanel.activeSelf)
            {
                dialoguePanel.SetActive(false);
                closed = true;
            }

            if (optionPanel != null && optionPanel.activeSelf)
            {
                optionPanel.SetActive(false);
                closed = true;
            }

            if (shopPanel != null && shopPanel.activeSelf)
            {
                shopPanel.SetActive(false);
                closed = true;
            }

            if (closed)
            {
                Time.timeScale = 1;
                Debug.Log("ESC 입력 → 모든 UI 닫기 완료");
            }
            if (shopInventory != null && shopInventory.activeSelf)
            {
                shopInventory.SetActive(false);
                closed = true;
            }
        }
    }
}
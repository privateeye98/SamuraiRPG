using UnityEngine;
using TMPro;

public class QuestPopupUI : MonoBehaviour
{
    public static QuestPopupUI instance;

    public TextMeshProUGUI popupText;
    public float displayDuration = 1.0f;
    private Coroutine currentRoutine;

    void Awake()
    {
        instance = this;
    }

    public void ShowProgress(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        popupText.text = message;
        gameObject.SetActive(true);
        currentRoutine = StartCoroutine(HideAfterDelay());
    }
    public void ShowAccept(Quest quest)
    {
        // �� ��¥�� �����̹Ƿ� ShowProgress ��Ȱ��
        ShowProgress($"<color=yellow>����Ʈ ����!</color>  {quest.data.questTitle}");
    }
    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false);
    }
}

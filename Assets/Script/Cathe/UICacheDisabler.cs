using UnityEngine;

public class UICacheDisabler : MonoBehaviour
{
    [SerializeField] GameObject[] panelsToDisable;

    void Start()
    {
        StartCoroutine(DisableAfterFrame());
    }

    System.Collections.IEnumerator DisableAfterFrame()
    {
        yield return null; // �� ������ ��� (Awake, Start ����)
        foreach (var panel in panelsToDisable)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }
}

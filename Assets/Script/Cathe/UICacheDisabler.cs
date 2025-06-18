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
        yield return null; // 한 프레임 대기 (Awake, Start 이후)
        foreach (var panel in panelsToDisable)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }
}

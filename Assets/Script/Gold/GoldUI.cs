using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;

     void Update()
    {
        if (goldText == null)
        {
            Debug.LogWarning("GoldUI: goldText �ʵ尡 ����Ǿ� ���� �ʽ��ϴ�!");
            return;
        }

        if (GoldManager.instance != null)
        {
            goldText.text = $"Gold: {GoldManager.instance.currentGold}";
        }
        else
        {
            goldText.text = "Gold: ?";
        }
    }
}

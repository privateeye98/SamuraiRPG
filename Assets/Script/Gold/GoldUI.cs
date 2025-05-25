using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;

     void Update()
    {
        if (goldText == null)
        {
            Debug.LogWarning("GoldUI: goldText 필드가 연결되어 있지 않습니다!");
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

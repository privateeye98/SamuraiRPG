using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;

     void Update()
    {
        if(GoldManager.instance != null)
        {
            goldText.text = $"Gold: {GoldManager.instance.currentGold}";
        }
    }
}

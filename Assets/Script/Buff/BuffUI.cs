using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI timerText;

    public void Setup(Sprite icon, float duration)
    {
        iconImage.sprite = icon;
        UpdateTime(duration);
    }

    public void UpdateTime(float time)
    {
        timerText.text = $"{time:F1}s";
    }
}

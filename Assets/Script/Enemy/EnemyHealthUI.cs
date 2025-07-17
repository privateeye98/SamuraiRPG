using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthUI : MonoBehaviour
{
    public Slider hpSlider;

    public void SetHP(int current, int max)
    {
        hpSlider.maxValue = max;
        hpSlider.value = current;
    }

}

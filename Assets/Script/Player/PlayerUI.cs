using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider mpSlider;
    [SerializeField] Slider expSlider;
    void Start()
    {
        if (PlayerHealth.instance != null)
        {
            hpSlider.maxValue = PlayerHealth.instance.maxHP;
            hpSlider.value = PlayerHealth.instance.currentHP;
        }

        if (PlayerMana.instance != null)
        {
            mpSlider.maxValue = PlayerMana.instance.maxMP;
            mpSlider.value = PlayerMana.instance.currentMP;
        }
        if (PlayerLevel.instance != null)
        {
            expSlider.maxValue = PlayerLevel.instance.GetRequiredExp(PlayerLevel.instance.currentLevel);
            expSlider.value = PlayerLevel.instance.currentExp;
        }

    }

    void Update()
    {
        if (PlayerHealth.instance != null)
            hpSlider.value = PlayerHealth.instance.currentHP;

        if (PlayerMana.instance != null)
            mpSlider.value = PlayerMana.instance.currentMP;
        if (PlayerLevel.instance != null)
        {
            expSlider.maxValue = PlayerLevel.instance.GetRequiredExp(PlayerLevel.instance.currentLevel);
            expSlider.value = PlayerLevel.instance.currentExp;
        }
    }

}

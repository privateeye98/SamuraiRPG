using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider mpSlider;

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
    }

    void Update()
    {
        if (PlayerHealth.instance != null)
            hpSlider.value = PlayerHealth.instance.currentHP;

        if (PlayerMana.instance != null)
            mpSlider.value = PlayerMana.instance.currentMP;
    }

}

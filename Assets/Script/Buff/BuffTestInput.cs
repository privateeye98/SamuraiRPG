using UnityEngine;
public class BuffTestInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            BuffManager.instance.ApplyBuff(BuffType.StrengthUp, 5f, 30f);

        if (Input.GetKeyDown(KeyCode.F2))
            BuffManager.instance.ApplyBuff(BuffType.DexterityUp, 5f, 20f);

        if (Input.GetKeyDown(KeyCode.F3))
            BuffManager.instance.ApplyBuff(BuffType.CritUp, 5f, 15f);

        if (Input.GetKeyDown(KeyCode.F4))
            BuffManager.instance.ApplyBuff(BuffType.ExpUp, 10f, 1.5f);

        if (Input.GetKeyDown(KeyCode.F5))
            BuffManager.instance.ApplyBuff(BuffType.SpeedUp, 5f, 3f);
    }
}
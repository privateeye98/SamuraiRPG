using UnityEngine;
public class BuffTestInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            BuffManager.instance.ApplyBuff(BuffType.StrengthUp, 300f, 30f);

        if (Input.GetKeyDown(KeyCode.F2))
            BuffManager.instance.ApplyBuff(BuffType.DexterityUp, 300f, 20f);

        if (Input.GetKeyDown(KeyCode.F3))
            BuffManager.instance.ApplyBuff(BuffType.CritUp, 300f, 15f);

        if (Input.GetKeyDown(KeyCode.F4))
            BuffManager.instance.ApplyBuff(BuffType.ExpUp, 300f, 1.5f);

        if (Input.GetKeyDown(KeyCode.F5))
            BuffManager.instance.ApplyBuff(BuffType.SpeedUp, 300f, 3f);
    }
}
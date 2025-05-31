using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    [Header("UI")]
    [SerializeField] GameObject buffUIPrefab;
    [SerializeField] Transform buffUIParent;
    [SerializeField] Sprite StrengthIcon, dexterityIcon, speedIcon, criticalIcon, expIcon;

    private Coroutine currentRoutine;
    private GameObject currentUI;
    private BuffType currentType;

    private PlayerStat stat;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        stat = PlayerStat.instance;
    }

    public void ApplyBuff(BuffType type, float duration, float value)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            RemoveStat(currentType);
            Destroy(currentUI);
        }

        currentRoutine = StartCoroutine(BuffRoutine(type, duration, value));
    }

    private IEnumerator BuffRoutine(BuffType type, float duration, float value)
    {
        ApplyStat(type, value);
        currentType = type;

        Sprite iconSprite = GetIcon(type);

        GameObject icon = Instantiate(buffUIPrefab);

        icon.transform.SetParent(buffUIParent, worldPositionStays: false);

        BuffUI ui = icon.GetComponent<BuffUI>();
        ui.Setup(iconSprite, duration);

        RectTransform rt = icon.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;    
        rt.localScale = Vector3.one;           

        currentUI = icon;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            ui.UpdateTime(duration - elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        RemoveStat(type);
        Destroy(currentUI);
        currentRoutine = null;
    }

    private void ApplyStat(BuffType type, float value)
    {
        switch (type)
        {
            case BuffType.StrengthUp: stat.strength += (int)value; break;
            case BuffType.DexterityUp: stat.dexterity += (int)value; break;
            case BuffType.CritUp: stat.critical += (int)value; break;
            case BuffType.ExpUp: stat.expMultiplier = value; break; // °æÇèÄ¡ °ö
            case BuffType.SpeedUp: Player.instance.maxSpeed += value; break;
        }
        stat.NotifyStatChanged();
    }

    private void RemoveStat(BuffType type)
    {
        switch (type)
        {
            case BuffType.StrengthUp: stat.strength -= 30; break;
            case BuffType.DexterityUp: stat.dexterity -= 20; break;
            case BuffType.CritUp: stat.critical -= 15; break;
            case BuffType.ExpUp: stat.expMultiplier = 1f; break;
            case BuffType.SpeedUp: Player.instance.maxSpeed -= 3f; break;
        }
        stat.NotifyStatChanged();
    }

    private Sprite GetIcon(BuffType type)
    {
        return type switch
        {
            BuffType.StrengthUp => StrengthIcon,
            BuffType.DexterityUp => dexterityIcon,
            BuffType.CritUp => criticalIcon,
            BuffType.ExpUp => expIcon,
            BuffType.SpeedUp => speedIcon,
            _ => null
        };
    }
}

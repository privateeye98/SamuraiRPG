using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    // 1) 晦獄 薑爾
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    [Header("式式 晦獄 薑爾 式式")]
    [Tooltip("嬴檜蠱 堅嶸 ID")]
    public int id;

    [Tooltip("檣漸饜葬縑 ル衛腆 檜葷")]
    public string itemName;

    [Tooltip("檣漸饜葬 塽 罐そ縑 餌辨й 嬴檜夔 蝶Щ塭檜お")]
    public Sprite icon;

    [Tooltip("嬴檜蠱 顫殮 (Consumable, Equipment, Quest 蛔)")]
    public ItemType type;

    [Tooltip("濰綠 だお (Head, Body, Weapon 蛔)")]
    public ItemPartType part;

    [TextArea(2, 5)]
    [Tooltip("罐そ 鼻縑 爾罹還 撲貲 僥掘")]
    public string description;

    [Tooltip("檣漸饜葬縑 論擊 熱 氈朝雖 罹睡 (模綠 嬴檜蠱虜)")]
    public bool isStackable = false;

    [Tooltip("譆渠 蝶鷗 熱 (蕨: ん暮 200偃梱雖)")]
    public int maxStack = 1;

    [Tooltip("鼻薄縑憮 っ衙 衛 �蜈鷍� 埤萄 綠徽 (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float sellRatio = 0.5f;

    [Tooltip("鼻薄縑憮 掘衙 衛 晦獄 陛問")]
    public int price;

    [Space(10)]
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    // 2) 雜辨 譆模 褻勒
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    [Header("式式 雜辨 譆模 褻勒 式式")]
    [Tooltip("雜辨 陛棟и 譆模 溯漣")]
    public int requiredLevel = 1;

    [Tooltip("雜辨 褻勒戲煎 в蹂и 蝶囌 跡煙 (蕨: STR ￣ 10)")]
    public StatRequirement[] requiredStats;

    [Serializable]
    public struct StatRequirement
    {
        [Tooltip("в蹂и 蝶囌 謙盟 (STR, DEX, CRIT 蛔)")]
        public StatType stat;
        [Tooltip("в蹂и 譆模 高")]
        public int value;
    }

    [Space(10)]
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    // 3) 晦獄 蝶囌 (Base Stats)
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    [Header("式式 晦獄 蝶囌 (雜辨 衛 堅薑 爾傘蝶) 式式")]
    [Tooltip("濰綠 雜辨 衛 蹺陛腎朝 奢問溘 (堅薑高)")]
    public int baseATK;

    [Tooltip("濰綠 雜辨 衛 蹺陛腎朝 寞橫溘 (堅薑高)")]
    public int baseDEF;

    [Tooltip("濰綠 雜辨 衛 蹺陛腎朝 譆渠 羹溘 (堅薑高)")]
    public int baseHP;

    [Tooltip("濰綠 雜辨 衛 蹺陛腎朝 譆渠 葆釭 (堅薑高)")]
    public int baseMP;

    [Tooltip("濰綠 雜辨 衛 蹺陛腎朝 STR (堅薑高)")]
    public int baseSTR;

    [Tooltip("濰綠 雜辨 衛 蹺陛腎朝 DEX (堅薑高)")]
    public int baseDEX;

    [Tooltip("濰綠 雜辨 衛 蹺陛腎朝 CRIT �捕� (堅薑高, ぷ撫お)")]
    public int baseCRIT;


    [Space(10)]
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    // 4) 鬼�� 溯漣渡 爾傘蝶 (Per-Level Bonuses)
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    [Header("式式 鬼�� 溯漣渡 爾傘蝶 (Per Level) 式式")]
    [Tooltip("鬼�� 1溯漣渡 蹺陛腎朝 奢問溘")]
    public int perLevelATK;

    [Tooltip("鬼�� 1溯漣渡 蹺陛腎朝 寞橫溘")]
    public int perLevelDEF;

    [Tooltip("鬼�� 1溯漣渡 蹺陛腎朝 羹溘")]
    public int perLevelHP;

    [Tooltip("鬼�� 1溯漣渡 蹺陛腎朝 葆釭")]
    public int perLevelMP;

    [Tooltip("鬼�� 1溯漣渡 蹺陛腎朝 STR")]
    public int perLevelSTR;

    [Tooltip("鬼�� 1溯漣渡 蹺陛腎朝 DEX")]
    public int perLevelDEX;

    [Tooltip("鬼�� 1溯漣渡 蹺陛腎朝 CRIT �捕� (ぷ撫お)")]
    public int perLevelCRIT;


    [Space(10)]
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    // 5) 鬼�� 婦溼 撲薑
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    [Header("式式 鬼�� 婦溼 撲薑 式式")]
    [Tooltip("嬴檜蠱 鬼�� 譆渠 溯漣")]
    public int maxLevel = 10;

    [Tooltip("鬼�� 綠辨 (溯漣渡 培п雖朝 旎擋)")]
    public int upgradeCost = 100;

    [Tooltip("溯漣滌 譆模 撩奢 �捕� (鬼�� 褒ぬ 衛縑紫 爾濰)")]
    public float minSuccessRate = 0.1f;

    [Tooltip("晦獄 鬼�� 撩奢 �捕� (蕨: 1⊥2溯漣 衛 50%)")]
    public float baseSuccessRate = 0.5f;

    [Tooltip("鬼�� 溯漣渡 ぬ割じ (蕨: 溯漣 ж釭渡 -1% 馬模)")]
    public float penaltyPerLevel = 0.01f;

    /// <summary>
    /// 褒薯 撩奢 �捕� 啗骯 (currentLevel 高擊 檣濠煎 嫡嬴 餌辨).
    /// </summary>
    public float GetSuccessRate(int currentLevel)
    {
        float rate = baseSuccessRate - (currentLevel * penaltyPerLevel);
        return Mathf.Clamp(rate, minSuccessRate, 1f);
    }


    [Space(10)]
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    // 6) 晦顫 樓撩 (模綠 嬴檜蠱 蛔)
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    [Header("式式 模綠 嬴檜蠱 樓撩 式式")]
    [Tooltip("��榆 (模綠 嬴檜蠱)")]
    public int healAmount;

    [Tooltip("葆釭 �蛹僩� (模綠 嬴檜蠱)")]
    public int ManaAmount;

    [Tooltip("罐そ縑 蹺陛煎 爾罹還 爾褻 薑爾 (в蹂 衛 �挫�)")]
    public StatType statType;  // (蕨: 罐そ縑 ＆檜 嬴檜蠱擎 CRIT �捕�擊 隸陛衛霾棲棻＊ 蛔)


    [Space(10)]
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    // 7) 楛顫歜辨 頂睡 в萄 (檣蝶め攪 喻轎 碳в蹂)
    // 式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式式
    [HideInInspector]
    public StatModifier[] bonusStats; // InventoryItem.PopulateBonusStats() 欽啗縑憮 瓣錶餵棲棻.

    [Serializable]
    public struct StatModifier
    {
        public StatType stat;
        public int amount; // Per-Level 爾傘蝶 高
    }
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // 1) ±âº» Á¤º¸
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ ±âº» Á¤º¸ ¦¡¦¡")]
    [Tooltip("¾ÆÀÌÅÛ °íÀ¯ ID")]
    public int id;

    [Tooltip("ÀÎº¥Åä¸®¿¡ Ç¥½ÃµÉ ÀÌ¸§")]
    public string itemName;

    [Tooltip("ÀÎº¥Åä¸® ¹× ÅøÆÁ¿¡ »ç¿ëÇÒ ¾ÆÀÌÄÜ ½ºÇÁ¶óÀÌÆ®")]
    public Sprite icon;

    [Tooltip("¾ÆÀÌÅÛ Å¸ÀÔ (Consumable, Equipment, Quest µî)")]
    public ItemType type;

    [Tooltip("Àåºñ ÆÄÆ® (Head, Body, Weapon µî)")]
    public ItemPartType part;

    [TextArea(2, 5)]
    [Tooltip("ÅøÆÁ »ó¿¡ º¸¿©ÁÙ ¼³¸í ¹®±¸")]
    public string description;

    [Tooltip("ÀÎº¥Åä¸®¿¡ ½×À» ¼ö ÀÖ´ÂÁö ¿©ºÎ (¼Òºñ ¾ÆÀÌÅÛ¸¸)")]
    public bool isStackable = false;

    [Tooltip("ÃÖ´ë ½ºÅÃ ¼ö (¿¹: Æ÷¼Ç 200°³±îÁö)")]
    public int maxStack = 1;

    [Tooltip("»óÁ¡¿¡¼­ ÆÇ¸Å ½Ã È¸¼öÇÒ °ñµå ºñÀ² (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float sellRatio = 0.5f;

    [Tooltip("»óÁ¡¿¡¼­ ±¸¸Å ½Ã ±âº» °¡°Ý")]
    public int price;

    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // 2) Âø¿ë ÃÖ¼Ò Á¶°Ç
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ Âø¿ë ÃÖ¼Ò Á¶°Ç ¦¡¦¡")]
    [Tooltip("Âø¿ë °¡´ÉÇÑ ÃÖ¼Ò ·¹º§")]
    public int requiredLevel = 1;

    [Tooltip("Âø¿ë Á¶°ÇÀ¸·Î ÇÊ¿äÇÑ ½ºÅÈ ¸ñ·Ï (¿¹: STR ¡Ã 10)")]
    public StatRequirement[] requiredStats;

    [Serializable]
    public struct StatRequirement
    {
        [Tooltip("ÇÊ¿äÇÑ ½ºÅÈ Á¾·ù (STR, DEX, CRIT µî)")]
        public StatType stat;
        [Tooltip("ÇÊ¿äÇÑ ÃÖ¼Ò °ª")]
        public int value;
    }

    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // 3) ±âº» ½ºÅÈ (Base Stats)
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ ±âº» ½ºÅÈ (Âø¿ë ½Ã °íÁ¤ º¸³Ê½º) ¦¡¦¡")]
    [Tooltip("Àåºñ Âø¿ë ½Ã Ãß°¡µÇ´Â °ø°Ý·Â (°íÁ¤°ª)")]
    public int baseATK;

    [Tooltip("Àåºñ Âø¿ë ½Ã Ãß°¡µÇ´Â ¹æ¾î·Â (°íÁ¤°ª)")]
    public int baseDEF;

    [Tooltip("Àåºñ Âø¿ë ½Ã Ãß°¡µÇ´Â ÃÖ´ë Ã¼·Â (°íÁ¤°ª)")]
    public int baseHP;

    [Tooltip("Àåºñ Âø¿ë ½Ã Ãß°¡µÇ´Â ÃÖ´ë ¸¶³ª (°íÁ¤°ª)")]
    public int baseMP;

    [Tooltip("Àåºñ Âø¿ë ½Ã Ãß°¡µÇ´Â STR (°íÁ¤°ª)")]
    public int baseSTR;

    [Tooltip("Àåºñ Âø¿ë ½Ã Ãß°¡µÇ´Â DEX (°íÁ¤°ª)")]
    public int baseDEX;

    [Tooltip("Àåºñ Âø¿ë ½Ã Ãß°¡µÇ´Â CRIT È®·ü (°íÁ¤°ª, ÆÛ¼¾Æ®)")]
    public int baseCRIT;


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // 4) °­È­ ·¹º§´ç º¸³Ê½º (Per-Level Bonuses)
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ °­È­ ·¹º§´ç º¸³Ê½º (Per Level) ¦¡¦¡")]
    [Tooltip("°­È­ 1·¹º§´ç Ãß°¡µÇ´Â °ø°Ý·Â")]
    public int perLevelATK;

    [Tooltip("°­È­ 1·¹º§´ç Ãß°¡µÇ´Â ¹æ¾î·Â")]
    public int perLevelDEF;

    [Tooltip("°­È­ 1·¹º§´ç Ãß°¡µÇ´Â Ã¼·Â")]
    public int perLevelHP;

    [Tooltip("°­È­ 1·¹º§´ç Ãß°¡µÇ´Â ¸¶³ª")]
    public int perLevelMP;

    [Tooltip("°­È­ 1·¹º§´ç Ãß°¡µÇ´Â STR")]
    public int perLevelSTR;

    [Tooltip("°­È­ 1·¹º§´ç Ãß°¡µÇ´Â DEX")]
    public int perLevelDEX;

    [Tooltip("°­È­ 1·¹º§´ç Ãß°¡µÇ´Â CRIT È®·ü (ÆÛ¼¾Æ®)")]
    public int perLevelCRIT;


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // 5) °­È­ °ü·Ã ¼³Á¤
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ °­È­ °ü·Ã ¼³Á¤ ¦¡¦¡")]
    [Tooltip("¾ÆÀÌÅÛ °­È­ ÃÖ´ë ·¹º§")]
    public int maxLevel = 10;

    [Tooltip("°­È­ ºñ¿ë (·¹º§´ç °öÇØÁö´Â ±Ý¾×)")]
    public int upgradeCost = 100;

    [Tooltip("·¹º§º° ÃÖ¼Ò ¼º°ø È®·ü (°­È­ ½ÇÆÐ ½Ã¿¡µµ º¸Àå)")]
    public float minSuccessRate = 0.1f;

    [Tooltip("±âº» °­È­ ¼º°ø È®·ü (¿¹: 1¡æ2·¹º§ ½Ã 50%)")]
    public float baseSuccessRate = 0.5f;

    [Tooltip("°­È­ ·¹º§´ç ÆÐ³ÎÆ¼ (¿¹: ·¹º§ ÇÏ³ª´ç -1% °¨¼Ò)")]
    public float penaltyPerLevel = 0.01f;

    /// <summary>
    /// ½ÇÁ¦ ¼º°ø È®·ü °è»ê (currentLevel °ªÀ» ÀÎÀÚ·Î ¹Þ¾Æ »ç¿ë).
    /// </summary>
    public float GetSuccessRate(int currentLevel)
    {
        float rate = baseSuccessRate - (currentLevel * penaltyPerLevel);
        return Mathf.Clamp(rate, minSuccessRate, 1f);
    }


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // 6) ±âÅ¸ ¼Ó¼º (¼Òºñ ¾ÆÀÌÅÛ µî)
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ ¼Òºñ ¾ÆÀÌÅÛ ¼Ó¼º ¦¡¦¡")]
    [Tooltip("Èú·® (¼Òºñ ¾ÆÀÌÅÛ)")]
    public int healAmount;

    [Tooltip("¸¶³ª È¸º¹·® (¼Òºñ ¾ÆÀÌÅÛ)")]
    public int ManaAmount;

    [Tooltip("ÅøÆÁ¿¡ Ãß°¡·Î º¸¿©ÁÙ º¸Á¶ Á¤º¸ (ÇÊ¿ä ½Ã È®Àå)")]
    public StatType statType;  // (¿¹: ÅøÆÁ¿¡ ¡®ÀÌ ¾ÆÀÌÅÛÀº CRIT È®·üÀ» Áõ°¡½ÃÅµ´Ï´Ù¡¯ µî)


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // 7) ·±Å¸ÀÓ¿ë ³»ºÎ ÇÊµå (ÀÎ½ºÆåÅÍ ³ëÃâ ºÒÇÊ¿ä)
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [HideInInspector]
    public StatModifier[] bonusStats; // InventoryItem.PopulateBonusStats() ´Ü°è¿¡¼­ Ã¤¿öÁý´Ï´Ù.

    [Serializable]
    public struct StatModifier
    {
        public StatType stat;
        public int amount; // Per-Level º¸³Ê½º °ª
    }
}

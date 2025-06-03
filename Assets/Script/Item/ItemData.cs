using System;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // ±âº» Á¤º¸
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ ±âº» Á¤º¸ ¦¡¦¡")]
    [Tooltip("¾ÆÀÌÅÛ °íÀ¯ ID")]
    public int id;

    [Tooltip("ÀÎº¥Åä¸®¿¡ Ç¥½ÃµÉ ÀÌ¸§")]
    public string itemName;

    [Tooltip("ÀÎº¥Åä¸® ¹× ÅøÆÁ¿¡ ¾µ ¾ÆÀÌÄÜ ½ºÇÁ¶óÀÌÆ®")]
    public Sprite icon;

    [Tooltip("¾ÆÀÌÅÛ Å¸ÀÔ : ¼Òºñ¿ë, Àåºñ¿ë µî")]
    public ItemType type;

    [Tooltip("Àåºñ ÆÄÆ®(Head, Body, Weapon µî)")]
    public ItemPartType part;

    [Tooltip("ÅøÆÁ¿¡ ±âº» ¼³¸í ¹®±¸(¿¹: '°­È­¸¦ ÅëÇØ ½ºÅÈÀ» ³ôÀÏ ¼ö ÀÖ½À´Ï´Ù.')")]
    [TextArea(2, 5)]
    public string description;

    [Tooltip("ÀÎº¥Åä¸®¿¡ ½×À» ¼ö ÀÖ´Â ÃÖ´ë °¹¼ö(¼Òºñ ¾ÆÀÌÅÛÀÎ °æ¿ì)")]
    public bool isStackable = false;

    [Tooltip("ÃÖ´ë ½ºÅÃ ¼ö(¿¹: Æ÷¼ÇÀÌ¶ó¸é 200°³±îÁö ½×ÀÎ´Ù µî)")]
    public int maxStack = 1;

    [Tooltip("ÆÇ¸Å ½Ã È¹µæ °ñµå ºñÀ²(0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float sellRatio = 0.5f;

    [Tooltip("»óÁ¡¿¡¼­ ±¸¸Å ½Ã ±âº» °¡°Ý")]
    public int price;


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // Âø¿ë ÃÖ¼Ò Á¶°Ç
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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
        [Tooltip("ÇØ´ç ½ºÅÈÀÌ °¡Á®¾ß ÇÏ´Â ÃÖ¼Ò °ª")]
        public int value;
    }


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // ±âº» ½ºÅÈ
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ ±âº» ½ºÅÈ ¦¡¦¡")]
    [Tooltip("ÀÌ Àåºñ¸¦ Âø¿ëÇßÀ» ¶§ ÇÑ ¹ø¿¡ Áõ°¡½ÃÅ°´Â °ø°Ý·Â(·¹º§ º¸Á¤ Àü)")]
    public int atk;

    [Tooltip("ÀÌ Àåºñ¸¦ Âø¿ëÇßÀ» ¶§ ÇÑ ¹ø¿¡ Áõ°¡½ÃÅ°´Â Ã¼·Â(·¹º§ º¸Á¤ Àü)")]
    public int hpBonusPerLevel;  // ½ÇÁ¦·Î´Â ·¹º§´ç º¸³Ê½ºÀÌ¹Ç·Î, ÀÌ ÀÌ¸§Àº '·¹º§´ç º¸³Ê½º'·Î ¹Ù²Ü ¼öµµ ÀÖ½À´Ï´Ù.

    [Tooltip("ÀÌ Àåºñ¸¦ Âø¿ëÇßÀ» ¶§ ÇÑ ¹ø¿¡ Áõ°¡½ÃÅ°´Â ¸¶³ª(·¹º§ º¸Á¤ Àü)")]
    public int mpBonusPerLevel;

    [Tooltip("ÀÌ Àåºñ¸¦ Âø¿ëÇßÀ» ¶§ ÇÑ ¹ø¿¡ Áõ°¡½ÃÅ°´Â STR(·¹º§ º¸Á¤ Àü)")]
    public int strBonusPerLevel;

    [Tooltip("ÀÌ Àåºñ¸¦ Âø¿ëÇßÀ» ¶§ ÇÑ ¹ø¿¡ Áõ°¡½ÃÅ°´Â DEX(·¹º§ º¸Á¤ Àü)")]
    public int dexBonusPerLevel;

    [Tooltip("ÀÌ Àåºñ¸¦ Âø¿ëÇßÀ» ¶§ ÇÑ ¹ø¿¡ Áõ°¡½ÃÅ°´Â CRIT È®·ü(·¹º§ º¸Á¤ Àü)")]
    public int critBonusPerLevel;


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // °­È­ °ü·Ã ¼³Á¤
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ °­È­ °ü·Ã ¼³Á¤ ¦¡¦¡")]
    [Tooltip("ÇöÀç ÀåºñÀÇ ·±Å¸ÀÓ °­È­ ·¹º§(½ÇÁ¦ °è»ê ½Ã¿¡´Â InventoryItem.levelÀ» »ç¿ë)")]
    public int level = 1;

    [Tooltip("°­È­ ÃÖ´ë ·¹º§")]
    public int maxLevel = 10;

    [Tooltip("°­È­ ºñ¿ë (·¹º§´ç °öÇØÁö´Â ±Ý¾×)")]
    public int upgradeCost = 100;

    [Tooltip("·¹º§º° ÃÖ¼Ò ¼º°ø È®·ü (°­È­°¡ ½ÇÆÐÇÏ´õ¶óµµ ÃÖ¼Ò º¸Àå)")]
    public float minSuccessRate = 0.1f;

    [Tooltip("±âº» °­È­ ¼º°ø È®·ü (1·¹º§¡æ2·¹º§ ½Ã 50% µî)")]
    public float baseSuccessRate = 0.5f;

    [Tooltip("°­È­ ·¹º§´ç ÆÐ³ÎÆ¼ (¿¹: ·¹º§´ç -1%)")]
    public float penaltyPerLevel = 0.01f;

    /// <summary>
    /// ÇöÀç ·¹º§À» ÀÎÀÚ·Î ¹Þ¾Æ ½ÇÁ¦ ¼º°ø È®·üÀ» °è»êÇÕ´Ï´Ù.
    /// ÁÖÀÇ: ³»ºÎÀûÀ¸·Î 'level' ÇÊµå¸¦ »ç¿ëÇÏÁö ¾Ê°í, È£Ãâ ½Ã ³Ñ°Ü¹ÞÀº °ª¸¸ ¹Ý¿µÇØ¾ß ÇÕ´Ï´Ù.
    /// </summary>
    public float GetSuccessRate(int currentLevel)
    {
        float rate = baseSuccessRate - (currentLevel * penaltyPerLevel);
        return Mathf.Clamp(rate, minSuccessRate, 1f);
    }


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // ±âÅ¸ ¾ÆÀÌÅÛ ¼Ó¼º (¼Òºñ ¾ÆÀÌÅÛ µî)
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [Header("¦¡¦¡ ±âÅ¸ ¼Ó¼º ¦¡¦¡")]
    [Tooltip("Èú·® (¼Òºñ ¾ÆÀÌÅÛÀÎ °æ¿ì)")]
    public int healAmount;

    [Tooltip("¸¶³ª È¸º¹·® (¼Òºñ ¾ÆÀÌÅÛÀÎ °æ¿ì)")]
    public int ManaAmount;

    [Tooltip("ÅøÆÁ¿¡ Ãß°¡·Î º¸¿©ÁÙ º¸Á¶ Á¤º¸ (ÇÊ¿ä ½Ã È®Àå)")]
    public StatType statType; // ¿¹: ÅøÆÁ¿¡ ¡®ÀÌ ¾ÆÀÌÅÛÀº CRIT È®·üÀ» Áõ°¡½ÃÅµ´Ï´Ù¡¯ µîÀ» Ç¥½ÃÇÒ ¶§ »ç¿ë


    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // ³»ºÎ·Î °ü¸®¸¸ ÇÏ´Â ¸®½ºÆ®(ÀÎ½ºÆåÅÍ ³ëÃâ ºÒÇÊ¿ä)
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [HideInInspector]
    public StatModifier[] bonusStats; // ·¹º§ 1´ç ÁÖ¾îÁö´Â º¸³Ê½º: InventoryItem.GetEnhancedStats()¿¡¼­ »ç¿ë

    [Serializable]
    public struct StatModifier
    {
        public StatType stat;
        public int amount;
    }


    [Space(10)]
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    // Âü°í¿ë/µð¹ö±ë¿ë ÇÊµå (ÆíÁý ±ÝÁö)
    // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
    [HideInInspector]
    public int tmpAttackValue; // (ÅøÆÁ °è»ê ½Ã ÀÓ½Ã·Î ¾µ ¼ö ÀÖÀ½)

    [HideInInspector]
    public bool isEquipped;    // ÀåÂø ¿©ºÎ Ã¼Å© ¿ëµµ (ÀÎ½ºÆåÅÍ¿¡ ¾È º¸ÀÌ°Ô ÇÔ)
}

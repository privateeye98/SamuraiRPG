using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class UpgradePanelToggle : MonoBehaviour
{
    [SerializeField] GameObject upgradePanel;
    [SerializeField] UpgradeUI upgradeUI;
    public Dictionary<ItemPartType, ItemData> GetUpgradeItems() => upgradeItems;

    // 기본 장비들 등록
    [SerializeField] ItemData head;
    [SerializeField] ItemData body;
    [SerializeField] ItemData leg;
    [SerializeField] ItemData shoe;
    [SerializeField] ItemData glove;
    [SerializeField] ItemData weapon;
    Dictionary<ItemPartType, ItemData> upgradeItems;
    void Start()
    {
        if(head == null || body == null || leg == null || shoe == null || glove == null || weapon == null)
        {
            Debug.LogError("패널에 할당되지 않은것이 있습니다.");
            return;
        }


        head.level = 1;
        body.level = 1;
        leg.level = 1;
        shoe.level = 1;
        glove.level = 1;
        weapon.level = 1;

        upgradeItems = new Dictionary<ItemPartType, ItemData>
        {
            [ItemPartType.Head] = head,
            [ItemPartType.Body] = body,
            [ItemPartType.Leg] = leg,
            [ItemPartType.Shoe] = shoe,
            [ItemPartType.Glove] = glove,
            [ItemPartType.Weapon] = weapon
        };

        foreach (var pair in upgradeItems)
        {
            var invItem = new InventoryItem(pair.Value, qty: 1, lv: pair.Value.level);
            EquipmentManager.instance.equippedItems[pair.Key] = invItem;
        }

        EquipmentUI.instance.RefreshUI();
        InventoryUI.instance.UpdateUI();

        var levels = upgradeItems.ToDictionary(p => p.Key, p => p.Value.level);
        EquipmentManager.instance.ApplyEquipmentStats(upgradeItems, levels);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            bool isActive = !upgradePanel.activeSelf;
            upgradePanel.SetActive(isActive);
            if (upgradePanel != null)
            {
                var equipDict = EquipmentManager.instance.equippedItems;

                var dataDict = equipDict.ToDictionary(kv => kv.Key, kv => kv.Value.itemData);

                var levelDict = equipDict.ToDictionary(
                    kv => kv.Key, kv => kv.Value.level
                    );

                upgradeUI.Open(dataDict, levelDict);
                
            }
        }
    }

}

using UnityEngine;
using System.Collections.Generic;
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
        upgradeItems = new Dictionary<ItemPartType, ItemData>
        {
            [ItemPartType.Head] = head,
            [ItemPartType.Body] = body,
            [ItemPartType.Leg] = leg,
            [ItemPartType.Shoe] = shoe,
            [ItemPartType.Glove] = glove,
            [ItemPartType.Weapon] = weapon
        };
        PlayerStat.instance?.ApplyEquipmentStats(upgradeItems);
    }
   void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (upgradePanel != null)
            {
                bool isActive = !upgradePanel.activeSelf;
                upgradePanel.SetActive(isActive);

                // 강화창이 열릴 때만 upgradeItems 전달
                if (isActive && upgradeUI != null)
                {
                    upgradeUI.Open(upgradeItems);
                }
            }
        }
    }

}

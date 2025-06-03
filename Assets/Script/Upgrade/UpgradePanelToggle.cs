using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradePanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private UpgradeUI upgradeUI;

    void Start()
    {
        // 시작 시 강화창 비활성화
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        // 한 프레임 지연 후 스탯 보너스 초기 적용 (UI가 모두 준비된 뒤)
        StartCoroutine(DelayedApplyEffects());
    }

    private IEnumerator DelayedApplyEffects()
    {
        yield return null;
        EquipmentManager.instance.ApplyEquipmentEffects();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            bool isActive = !upgradePanel.activeSelf;
            upgradePanel.SetActive(isActive);

            if (isActive)
            {
                // 강화창을 열 때 장비 매니저의 최신 equippedItems를 전달
                upgradeUI.Open(EquipmentManager.instance.equippedItems);
            }
        }
    }
}

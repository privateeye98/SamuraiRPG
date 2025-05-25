using UnityEngine;

[CreateAssetMenu(menuName = "Game/DropTable")]
public class DropTable : ScriptableObject
{
    [System.Serializable]
    public struct DropEntry
    {
        public ItemData itemData;     // 드롭할 아이템 데이터
        [Range(0f, 1f)]
        public float dropChance;
    }
    public DropEntry[] entries;
    public GameObject pickupPrefab;  // 제네릭 ItemPickup 프리팹
}
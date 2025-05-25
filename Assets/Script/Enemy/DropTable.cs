using UnityEngine;

[CreateAssetMenu(menuName = "Game/DropTable")]
public class DropTable : ScriptableObject
{
    [System.Serializable]
    public struct DropEntry
    {
        public ItemData itemData;     // ����� ������ ������
        [Range(0f, 1f)]
        public float dropChance;
    }
    public DropEntry[] entries;
    public GameObject pickupPrefab;  // ���׸� ItemPickup ������
}
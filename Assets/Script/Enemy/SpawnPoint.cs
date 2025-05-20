using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    [Header("���� ����")]
    public GameObject enemyPrefab;
    public int maxSpawnCount = 3;
    public float spawnDelay = 5f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // ���� ����ִ� ���� �� üũ
            CleanupNullEnemies();

            if (spawnedEnemies.Count < maxSpawnCount)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                spawnedEnemies.Add(enemy);

                // ������ ��Ͽ��� ���ŵǰ� �̺�Ʈ ����
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.onDeath += () =>
                    {
                        spawnedEnemies.Remove(enemy);
                    };
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void CleanupNullEnemies()
    {
        // �׾ Destroy�� ������Ʈ ����
        spawnedEnemies.RemoveAll(e => e == null);
    }
}

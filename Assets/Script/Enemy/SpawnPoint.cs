using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    [Header("스폰 설정")]
    public GameObject enemyPrefab;
    public int maxSpawnCount = 3;
    public float spawnDelay = 5f;
    [Header("스폰 위치 랜덤 범위")]

    public float rangeX = 2f;
    public float rangeY = 0f;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            CleanupNullEnemies();

            int spawnCount = maxSpawnCount - spawnedEnemies.Count;

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPos = transform.position + new Vector3(
                    Random.Range(-rangeX, rangeX),
                    Random.Range(-rangeY, rangeY),
                    0f
                );

                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawnedEnemies.Add(enemy);

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
        // 죽어서 Destroy된 오브젝트 정리
        spawnedEnemies.RemoveAll(e => e == null);
    }
}

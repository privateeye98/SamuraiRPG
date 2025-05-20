using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    [Header("스폰 설정")]
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
            // 현재 살아있는 몬스터 수 체크
            CleanupNullEnemies();

            if (spawnedEnemies.Count < maxSpawnCount)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                spawnedEnemies.Add(enemy);

                // 죽으면 목록에서 제거되게 이벤트 연결
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

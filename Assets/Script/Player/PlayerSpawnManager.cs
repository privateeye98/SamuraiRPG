using UnityEngine;
using System.Collections;

public class PlayerSpawnManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SpawnWithFreeze());
    }

    IEnumerator SpawnWithFreeze()
    {
        yield return null; // 1프레임 대기

        string spawnName = PlayerPrefs.GetString("SpawnPoint", "SpawnPoint_Default");
        GameObject point = GameObject.Find(spawnName);
        GameObject player = GameObject.FindWithTag("Player");

        if (point != null && player != null)
        {
            // ✅ Rigidbody 잠깐 멈춤
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true; // 중력 안 먹게 함
            }

            player.transform.position = point.transform.position + Vector3.up * 0.1f;
            Debug.Log($"[스폰] {spawnName} 위치로 이동됨");

            yield return new WaitForEndOfFrame(); // 1프레임 더 대기

            if (rb != null)
            {
                rb.isKinematic = false; // 다시 정상 동작
            }
        }
        else
        {
            Debug.LogWarning("❌ SpawnPoint 또는 Player 찾기 실패");
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawnManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SpawnWithFreeze());
    }

    IEnumerator SpawnWithFreeze()
    {
        yield return null;

        string spawnName = PlayerPrefs.GetString("SpawnPoint", "SpawnPoint_Default");
        GameObject point = GameObject.Find(spawnName);
        GameObject player = GameObject.FindWithTag("Player");

        if (point != null && player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true;
            }

            player.transform.position = point.transform.position + Vector3.up * 0.1f;
            Debug.Log($"[스폰 완료] {spawnName} 위치로 이동");

            yield return new WaitForEndOfFrame();

            if (rb != null)
                rb.isKinematic = false;
        }
        else
        {
            Debug.LogWarning("SpawnPoint 또는 Player 찾기 실패");
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class Meteor : MonoBehaviour
{
    public int damage = 30;
    public float fallSpeed = 15f;
    public float lifetime = 5f;
    private Vector3 moveDirection;
    void Start()
    {
        moveDirection = new Vector3(Random.Range(-1.5f, -0.5f), -1f, 0f).normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += moveDirection * fallSpeed * Time.deltaTime;
    }
    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.TryGetComponent<IDamageable>(out var target))
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            target.TakeDamage(damage, transform.position, dir);

            Vector3 top = other.bounds.center + Vector3.up * 1f;
            DamageTextSpawner.I.Spawn(damage, top, false);

            Destroy(gameObject);
        }
    }
}

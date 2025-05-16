using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int dmg, Vector2 hitPoint, Vector2 hitDir);
}
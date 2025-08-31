using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyProjectile : MonoBehaviour
{
    public int damage = 1;
    public float life = 5f;

    void Awake()
    {
        // Make sure we behave like a trigger projectile
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // Auto-cleanup
        if (life > 0f) Destroy(gameObject, life);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Hit the player no matter which child collider we touched
        var hp = other.GetComponentInParent<Player_health>();
        if (hp != null)
        {
            hp.GetDamage(damage);
            Destroy(gameObject);
            return;
        }

        // (Optional) if we hit solid world, clean up too
        if (!other.isTrigger) Destroy(gameObject);
    }
}

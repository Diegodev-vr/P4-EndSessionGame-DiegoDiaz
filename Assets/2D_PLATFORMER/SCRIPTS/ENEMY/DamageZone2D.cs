using UnityEngine;

public class DamageZone2D : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float damageCooldown = 0.5f;

    private float lastHitTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Prevent spam damage every frame
        if (Time.time < lastHitTime + damageCooldown) return;

        lastHitTime = Time.time;

        // DAMAGE
        GameManager.Instance.TakeDamage(damageAmount);

        // KNOCKBACK
        Player_1_Movement player = other.GetComponent<Player_1_Movement>();

        if (player != null)
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            player.ApplyKnockback(direction * knockbackForce);
        }
    }
}
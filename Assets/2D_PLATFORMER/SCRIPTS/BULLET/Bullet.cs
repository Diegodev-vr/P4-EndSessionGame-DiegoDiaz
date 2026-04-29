using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector2 direction)
    {
        if (rb == null) return;

        rb.gravityScale = 0f;
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Target target))
            return;

        TargetManager.Instance.HandleBulletHit(collision.gameObject);
        Destroy(gameObject);
    }

}
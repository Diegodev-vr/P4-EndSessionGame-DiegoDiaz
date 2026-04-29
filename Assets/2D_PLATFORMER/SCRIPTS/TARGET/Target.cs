using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int health = 3;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        TargetManager.Instance.RegisterTarget();
    }

    public void TakeDamage(int amount)
    {
        if (health <= 0) return;

        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        spriteRenderer.color = Color.gray;

        TargetManager.Instance.NotifyTargetDestroyed();

        gameObject.SetActive(false);
    }
}
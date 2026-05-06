using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private GameObject target;
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
        GameManager.Instance.PlayZombie();
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.PlayExplosion();
        //spriteRenderer.color = Color.gray;

        TargetManager.Instance.NotifyTargetDestroyed();

        target.SetActive(false);
    }
}
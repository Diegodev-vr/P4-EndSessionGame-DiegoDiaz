using UnityEngine;

public class Enemy_Chase2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float loseTargetDelay = 0.8f;
    [SerializeField] private float reengageDelay = 1.2f; // 🔥 NEW COOLDOWN

    private Transform player;
    private bool isWalking = false;

    private float lastSeenTime;
    private float lastLostTime;

    private void FixedUpdate()
    {
        bool canChase = Time.time > lastLostTime + reengageDelay;

        // 🧠 Only chase if cooldown passed
        if (player != null && canChase && Time.time < lastSeenTime + loseTargetDelay)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);

            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

            transform.localScale = direction > 0 ? Vector3.one : new Vector3(-1, 1, 1);

            if (!isWalking)
            {
                animator.SetTrigger("Walk");
                isWalking = true;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (isWalking)
            {
                animator.SetTrigger("Idle");
                isWalking = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            lastSeenTime = Time.time; // constantly refresh vision
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lastLostTime = Time.time; // 🔥 start cooldown
        }
    }
}
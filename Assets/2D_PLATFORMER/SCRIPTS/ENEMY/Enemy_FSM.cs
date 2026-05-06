using UnityEngine;

public class Enemy_FSM : MonoBehaviour
{
    // Declare the states
    public enum EnemyState { Idle, Chase }

    [Header("FSM Status")]
    [SerializeField] private EnemyState currentState = EnemyState.Idle;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;
    
    [SerializeField] private float loseTargetDelay = 0.8f;
    [SerializeField] private float reengageDelay = 1.2f;

    private Transform player;
    private float lastSeenTime;
    private float lastLostTime;

    private void FixedUpdate()
    {
        // 1. Evaluate State decides WHERE we are in the diagram
        EvaluateState();

        // 2. Switch decides WHAT code to run
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate();
                break;
        }
    }

    private void EvaluateState()
    {
        // Keep your original Timer logic for the transition conditions
        bool canChase = Time.time > lastLostTime + reengageDelay;
        bool withinVisionWindow = Time.time < lastSeenTime + loseTargetDelay;

        if (player != null && canChase && withinVisionWindow)
        {
           
            currentState = EnemyState.Chase; // Transition to Chase[cite: 1]
        }
        else
        {
            currentState = EnemyState.Idle; // Transition to Idle[cite: 1]
        }
    }

    private void IdleUpdate()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        
        // Instead of a bool 'isWalking', the State handles the animation[cite: 1]
        animator.SetTrigger("Idle");
    }

    private void ChaseUpdate()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        transform.localScale = direction > 0 ? Vector3.one : new Vector3(-1, 1, 1);
        
        animator.SetTrigger("Walk");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            player = other.transform;
            lastSeenTime = Time.time; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lastLostTime = Time.time; 
        }
    }
}
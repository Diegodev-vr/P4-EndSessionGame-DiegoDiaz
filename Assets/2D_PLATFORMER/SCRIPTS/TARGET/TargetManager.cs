using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    private int targetCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterTarget()
    {
        targetCount++;
    }

    public void NotifyTargetDestroyed()
    {
        targetCount--;
        Debug.Log("Targets remaining: " + targetCount);
    }

    // 🔥 THIS is the new part
    public void HandleBulletHit(GameObject hitObject)
    {
        if (hitObject.TryGetComponent(out Target target))
        {
            target.TakeDamage(1);
        }
    }
}
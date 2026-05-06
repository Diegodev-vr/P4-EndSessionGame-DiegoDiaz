using UnityEngine;
using System;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    private int targetCount;

    public event Action<int> OnTargetCountChanged;

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
        OnTargetCountChanged?.Invoke(targetCount);
    }

    public void NotifyTargetDestroyed()
    {
        targetCount--;
        OnTargetCountChanged?.Invoke(targetCount);

        Debug.Log("Targets remaining: " + targetCount);
    }

    public void HandleBulletHit(GameObject hitObject)
    {
        if (hitObject.TryGetComponent(out Target target))
        {
            target.TakeDamage(1);
        }
    }
}
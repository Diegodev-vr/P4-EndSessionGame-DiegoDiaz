using TMPro;
using UnityEngine;

public class TargetsCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private string prefix = "Targets: ";

    private void Awake()
    {
        if (targetText == null && !TryGetComponent(out targetText))
        {
            Debug.LogWarning("TargetsCounterUI requires TMP_Text");
            enabled = false;
        }
    }

    private void Start()
    {
        if (TargetManager.Instance != null)
        {
            TargetManager.Instance.OnTargetCountChanged += UpdateTargets;
        }
    }

    private void OnDestroy()
    {
        if (TargetManager.Instance != null)
        {
            TargetManager.Instance.OnTargetCountChanged -= UpdateTargets;
        }
    }

    private void UpdateTargets(int count)
    {
        targetText.text = prefix + count;
    }
}
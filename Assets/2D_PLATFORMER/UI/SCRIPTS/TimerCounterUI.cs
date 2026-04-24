using TMPro;
using UnityEngine;

public class TimerCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private string prefix = "Time: ";

    private void Awake()
    {
        ///// error handling in case the reference is not set in the inspector, also try to get the TMP_Text component from the same game object
        if (TimerText == null && !TryGetComponent(out TimerText))
        {
            Debug.LogWarning($"TimerCounterUI on {gameObject.name} requires a TMP_Text component. Disabling TimerCounterUI.");
            enabled = false;
        }
    }

    private void Start()
    {
        ///// subscribe to the OnTimerChanged event from the GameManager, so that the text will update whenever the timer changes
        if (GameManager.Instance != null)
            GameManager.Instance.OnTimerChanged += UpdateTimerText;

    }

    private void OnDestroy()
    {
        ///// unsubscribe from the OnTimerChanged event when this UI element is destroyed, to avoid memory leaks and null reference exceptions
        if (GameManager.Instance != null)
            GameManager.Instance.OnTimerChanged -= UpdateTimerText;
    }

    private void UpdateTimerText(float seconds)
    {
        ///// update the text to display the remaining time in seconds, with the optional prefix
        ///// seconds is a float value that represents the remaining time, we can round it to an
        /////integer for display purposes using Mathf.CeilToInt to always round up to the nearest whole second
        ///// seconds come from the GameManager's OnTimerChanged event, which is invoked in the Update function
        ///// of the GameManager whenever the timer changes so this function will be called every frame while
        ///// the timer is running, and it will update the text to show the current remaining time in seconds with the prefix
        if (TimerText == null) return;
        TimerText.text = prefix + Mathf.CeilToInt(seconds).ToString();
    }
}
using TMPro;
using UnityEngine;

public class PointsCounterUI : MonoBehaviour
{
    ///// reference to the TMP_Text component that will display the points count, this will be set in the inspector
    [SerializeField] private TMP_Text PointsText;
    ///// optional prefix for the points text, this will be displayed before the points count, you can set it in the inspector
    /////or leave it empty if you don't want a prefix
    [SerializeField] private string prefix = "Points: ";


    private void Awake()
    {
        ///// error handling in case the reference is not set in the inspector, also try to get the TMP_Text component from the same game object
        if(PointsText == null && !TryGetComponent(out PointsText))
        {
            Debug.LogWarning($"PointsCounterUI on {gameObject.name} requires a TMP_Text component. Disabling PointsCounterUI.");
            enabled = false;
        }
    }

    private void Start()
    {
        ///// subscribe to the OnPointsChanged event from the GameManager, so that the text will update whenever the player's points count changes
        if (GameManager.Instance != null)
        {
            ///// when receiving the OnPointsChanged event, the UpdatePointsText function will be called
            GameManager.Instance.OnPointsChanged += UpdatePointsText;
        }
        ///// initialize the points text with the current points count, in case the player starts with some points or if the points count is not zero at the start of the game
        UpdatePointsText(0);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPointsChanged -= UpdatePointsText;
        }
    }

    ///// this function recieve an integer parameter that represents the new points count, and update the text to display it with the optional prefix
    private void UpdatePointsText(int newPointsCount)
    {
        ///// update the text to display the new points count, with the optional prefix
        PointsText.text = prefix + newPointsCount;
    }
}

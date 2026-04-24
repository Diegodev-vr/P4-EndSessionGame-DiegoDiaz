using UnityEngine;
using UnityEngine.UI;

public class HealthSliderUI : MonoBehaviour
{
    ///// reference to the health slider component, this will be set in the inspector
    [SerializeField] private Slider m_HealthSlider;
    
    void Start()
    {
        ///// error handling in case the reference is not set in the inspector, also try to get the slider component from the same game object
        if(m_HealthSlider == null && !TryGetComponent(out m_HealthSlider))
        {
            Debug.LogError($"HealthBarUI on {gameObject.name} requires a Slider component.");
        }

        ///// initialize the slider values, the slider will represent the normalized health value
        ///// (current health divided by max health) so it will range from 0 to 1
       
        m_HealthSlider.value = 1f; // start at full health
        m_HealthSlider.minValue = 0f;
        m_HealthSlider.maxValue = 1f; // normalized health value
        m_HealthSlider.interactable = false; // make sure the slider is not interactable by the player

        ///// subscribe to the OnHealthChanged event from the GameManager, so that the slider will update whenever the player's health changes
        if (GameManager.Instance != null)
            ///// when receiving the OnHealthChanged event, the UpdateHealthBar function will be called
            ///// with the normalized health value as a parameter, so that the slider can update accordingly
            GameManager.Instance.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        ///// unsubscribe from the OnHealthChanged event when this UI element is destroyed, to avoid memory leaks and null reference exceptions
        if (GameManager.Instance != null)
            ///// stop listening to the OnHealthChanged event when this UI element is destroyed,
            ///// so that it won't try to update the slider when the player's health changes after this UI element is gone
            GameManager.Instance.OnHealthChanged -= UpdateHealthBar;
    }
    ///// this function receive a float parameter that represents the new normalized health value, and update the slider to reflect it
    void UpdateHealthBar(float normalizedHealth)
    {
        ///// update the slider value to reflect the player's current health,
        ///// the normalizedHealth parameter is a value between 0 and 1 that represents the percentage of health remaining
        m_HealthSlider.value = normalizedHealth;
    }
}
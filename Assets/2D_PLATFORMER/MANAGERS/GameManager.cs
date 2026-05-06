using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    ///// singleton pattern for easy access from other scripts, like the UI scripts
    ///// could be access from other scripts like this: GameManager.Instance.TakeDamage(10f);
    ///// subscribe and unsubcribe tho the events like this:
    ///// GameManager.Instance.OnHealthChanged += UpdateHealthBar; and GameManager.Instance.OnHealthChanged -= UpdateHealthBar;
    public static GameManager Instance { get; private set; }

    ///// variables to track the player's health and points
    [Header("Player Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float m_CurrentHealth;
    private int m_PointsCount;

    ///// variables for the timer, you can adjust the countdown time in the inspector
    ////// assign a TMP_Text component to display the timer
    [Header("Timer")]
    [SerializeField] private float countdownTime = 30f;
    private float m_CurrentTime;
    private bool timerRunning = false;

    ///// events to notify the UI when the health or points change, using Action from System namespace
    ///// the float parameter for OnHealthChanged is the normalized health value
    ///// (current health divided by max health) so that the UI can easily update the health bar slider

    public event Action<float> OnHealthChanged;
    public event Action<int> OnPointsChanged;

    ///// event to notify the UI when the timer changes
    public event Action<float> OnTimerChanged;

    ///// events for sound effects, these will be called by the enemy scripts when they hit the player, shoot, etc.
    public event Action OnPlayerHurt;
    public event Action OnGunShot;
    public event Action OnZombie;
    public event Action OnExplosion;
    public event Action OnGameStart;

    ///// these functions will be called by the enemy scripts to trigger the sound effects, they simply invoke the corresponding events
    public void PlayGunShot() => OnGunShot?.Invoke();
    public void PlayZombie() => OnZombie?.Invoke();
    public void PlayExplosion() => OnExplosion?.Invoke();
    public void StartGame() => OnGameStart?.Invoke();
    private void Awake()
    {
        ///// implement the singleton pattern, make sure there is only one instance of the GameManager in the scene
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        ///// initialize the player's health and points at the start of the game
        m_CurrentHealth = maxHealth;

        ///// initialize the timer
        m_CurrentTime = countdownTime;
    }

    private void Update()
    {
        if (!timerRunning) return;

        m_CurrentTime -= Time.deltaTime;

        // send current seconds to UI
        OnTimerChanged?.Invoke(Mathf.Max(m_CurrentTime, 0f));

        // restart when timer reaches 0
        if (m_CurrentTime <= 0f)
            RestartLevel();
    }

    ///// function to apply damage to the player, this will be called by the enemy scripts when they hit the player
    public void TakeDamage(float amount)
    {
        if (amount == 0) return;
        ///// clamp the current health between 0 and max health, then invoke the OnHealthChanged event to update the UI
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth - amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(m_CurrentHealth / maxHealth);

        ///// trigger the OnPlayerHurt event to play the hurt sound effect
        OnPlayerHurt?.Invoke();

        ///// check if the player's health has dropped to 0 or below, if so, restart the level
        if (m_CurrentHealth <= 0f)
        {
            RestartLevel();
        }
    }
    ///// function to heal the player, this will be called by the healing item scripts
    public void Heal(float amount)
    {
        if (amount == 0) return;
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(m_CurrentHealth / maxHealth);
    }
    // TODO: increment m_PointsCount, fire OnPointsChanged, Debug.Log
    public void AddPoint(int amount)
    {
        if (amount == 0) return;
        m_PointsCount += amount;
        OnPointsChanged?.Invoke(m_PointsCount);
        Debug.Log($"Points count: {m_PointsCount}");
    }
    ///// these context menu functions are for testing purposes,
    ///// they will appear in the inspector when you right click on the component, so you can easily test the TakeDamage,
    ///// Heal and AddPoint functions without having to set up the enemy or healing item scripts
    ///// this could be called from other script like this:
    ///// GameManager.Instance.TestTakeDamage(); but it's more convenient to call it from the inspector for quick testing

    [ContextMenu("Test: Take 10 Damage")]
    public void TestTakeDamage() => TakeDamage(10f);

    [ContextMenu("Test: Heal 20")]
    public void TestHeal() => Heal(20f);

    [ContextMenu("Test: Add Point")]
    public void TestAddPoint() => AddPoint(1);

    ///// function to restart the level, this will be called when the player's health drops to 0 or below
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    ///// function to start the timer, this will be called at the start of the level or when the player respawns
    public void StartTimer()
    {
        ///// reset the timer to the initial countdown time and start counting down
        m_CurrentTime = countdownTime;
        timerRunning = true;

        ///// immediately send current timer to UI
        ///// m_CurrentTime is reset to countdownTime, so this will send the initial countdown time to the UI when the timer starts
        OnTimerChanged?.Invoke(m_CurrentTime);

    }
}
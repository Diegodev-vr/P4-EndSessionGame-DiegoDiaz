using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip gameMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip gunShoot;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip zombie;
    [SerializeField] private AudioClip explosion;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Subscribe to GameManager events
        GameManager.Instance.OnPlayerHurt += PlayHurt;
        GameManager.Instance.OnGunShot += PlayGun;
        GameManager.Instance.OnZombie += PlayZombie;
        GameManager.Instance.OnExplosion += PlayExplosion;
        GameManager.Instance.OnGameStart += PlayGameMusic;

        PlayIntroMusic(); // start intro by default
    }

    void OnDestroy()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnPlayerHurt -= PlayHurt;
        GameManager.Instance.OnGunShot -= PlayGun;
        GameManager.Instance.OnZombie -= PlayZombie;
        GameManager.Instance.OnExplosion -= PlayExplosion;
        GameManager.Instance.OnGameStart -= PlayGameMusic;
    }

    // ===== MUSIC =====
    void PlayIntroMusic()
    {
        PlayMusic(introMusic);
    }

    void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // ===== SFX =====
    void PlayGun() => sfxSource.PlayOneShot(gunShoot);
    void PlayHurt() => sfxSource.PlayOneShot(hurt);
    void PlayZombie() => sfxSource.PlayOneShot(zombie);
    void PlayExplosion() => sfxSource.PlayOneShot(explosion);
}
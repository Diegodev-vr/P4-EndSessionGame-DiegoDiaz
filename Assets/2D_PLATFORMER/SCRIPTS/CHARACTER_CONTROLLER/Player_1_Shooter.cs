using UnityEngine;
using UnityEngine.InputSystem;

public class Player_1_Shooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.5f;

    private float nextShootTime = 0f;


    private Input_Actions_Platformer Input_Actions_Platformer;
    private InputAction fire;

    private void Awake()
    {
        Input_Actions_Platformer = new Input_Actions_Platformer();

    }

    private void OnEnable()
    {
        fire = Input_Actions_Platformer.Player_1.Fire;

        fire.Enable();
        fire.performed += Shoot;
    }

    private void OnDisable()
    {
        fire.performed -= Shoot;
        fire.Disable();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (Time.time < nextShootTime) return;

        nextShootTime = Time.time + shootCooldown;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (bullet.TryGetComponent(out Bullet bulletScript))
        {
            GameManager.Instance.PlayGunShot();
            
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            bulletScript.Initialize(direction);
        }
    }
}
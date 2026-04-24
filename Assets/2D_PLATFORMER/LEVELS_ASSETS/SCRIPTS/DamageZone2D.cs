using UnityEngine;
public class DamageZone2D : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        ///// when the player enters the damage zone, call the TakeDamage function from
        ///// the GameManager to reduce the player's health
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(damageAmount);
        }
    }
}
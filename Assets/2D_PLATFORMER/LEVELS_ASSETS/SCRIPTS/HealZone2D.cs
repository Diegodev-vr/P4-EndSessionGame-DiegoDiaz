using UnityEngine;
public class HealZone2D : MonoBehaviour
{
    ///// this script is very similar to the DamageZone2D, but instead of dealing damage
    ///// it will heal the player when they enter the trigger zone
    [SerializeField] private float healAmount = 10f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        ///// Check if the object hitting this is the player    
        if (other.CompareTag("Player"))
        {
            ///// call the Heal function on the GameManager to increase the player's health by the specified healAmount
            GameManager.Instance.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
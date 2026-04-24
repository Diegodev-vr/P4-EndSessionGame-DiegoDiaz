using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private ItemDataSO m_type;

    ///// when the player collides with the item pickup, try to add it to their inventory and destroy the pickup if successful
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ///// try to get the PlayerInventory component from the player object, and if it exists,
            ///// call the PickUp function to add the item to the inventory
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                ///// try to pick up the item, and if successful, destroy the pickup object
                bool pickedUp = inventory.PickUp(m_type);
                if (pickedUp)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

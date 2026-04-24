using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    ///// this script will handle the UI for the player's inventory,
    ///// displaying the items currently held in the inventory and updating the UI when items are added or removed
    [Header("References")]
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotsParent;

    private void Awake()
    {
        if (_playerInventory == null)
        {
            Debug.LogError("[InventoryUI] PlayerInventory reference is missing!");
            return;
        }

        if (_slotPrefab == null)
        {
            Debug.LogError("[InventoryUI] Slot prefab reference is missing!");
            return;
        }

        if (_slotsParent == null)
        {
            Debug.LogError("[InventoryUI] Slots parent reference is missing!");
            return;
        }

    }
    private void Start()
    {
        RefreshUI();
    }

    ///// this function will be called to refresh the inventory UI
    ///// by clearing the existing slots and creating new ones based
    ///// on the current contents of the player's inventory
    public void RefreshUI()
    {
        // Clear existing slots
        foreach (Transform child in _slotsParent)
        {
            Destroy(child.gameObject);
        }

        // Create new slots based on current inventory
        foreach (ItemDataSO item in _playerInventory.HeldItems)
        {
            ///// instantiate a new slot prefab for each item in the inventory, and set its data to display the item information
            GameObject slot = Instantiate(_slotPrefab, _slotsParent);
            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                slotUI.SetData(item);
            }
            else
            {
                Debug.LogError("[InventoryUI] Slot prefab is missing InventorySlotUI component!");
            }
        }
    }
}

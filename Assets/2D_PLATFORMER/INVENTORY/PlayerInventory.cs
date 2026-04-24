
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    // ── Inspector Setup ─────────────────────────────────────
    [Header("Settings")]
    [SerializeField] private int _maxCapacity = 10;

    [Header("Events")]
    [SerializeField] private UnityEvent _onInventoryChanged;

    // ── Private State ────────────────────────────────────────
    private List<ItemDataSO> m_heldItems;

    // ── Public Read-Only ─────────────────────────────────────
    public int Count => m_heldItems.Count;
    public bool IsFull => m_heldItems.Count >= _maxCapacity;

    ///// returns a read-only list of the items currently held in the inventory,
    ///// so that other scripts can access the inventory contents without being able to modify it directly
    public List<ItemDataSO> HeldItems => m_heldItems;

    // ── Lifecycle ────────────────────────────────────────────
    private void Awake()
    {
        m_heldItems = new List<ItemDataSO>();
    }

    // ── Public API ───────────────────────────────────────────

    /// <summary>
    /// Add an item to the inventory.
    /// Returns false if inventory is full.
    /// </summary>
    public bool PickUp(ItemDataSO type)
    {
        // Guard: full inventory
        if (IsFull)
        {
            Debug.Log("[Inventory] Full — cannot pick up more items.");
            return false;
        }

        m_heldItems.Add(type);
        _onInventoryChanged?.Invoke();

        // Look up data from database for confirmation log
        /*if (ItemDatabase.Instance != null &&
            ItemDatabase.Instance.TryGetItemData(type, out ItemData data))
        {
            Debug.Log($"[Inventory] Picked up: {data.name}");
        }*/

        return true;
    }

    /// <summary>
    /// Use and remove the first item of a given type.
    /// Returns false if not found.
    /// </summary>
   
    public bool UseItem(ItemDataSO type)
    {
        // Guard: item not in inventory
        if (!m_heldItems.Contains(type))
        {
            Debug.Log($"[Inventory] {type} not in inventory.");
            return false;
        }

        m_heldItems.Remove(type);
        _onInventoryChanged?.Invoke();
        Debug.Log($"[Inventory] Used: {type}");
        return true;
    }

    /// <summary>
    /// Use and remove the first item in the list (index 0).
    /// Returns false if inventory is empty.
    /// </summary>
    /// 
    [ContextMenu("Use first item")]
    public bool UseFirst()
    {
        // Guard: empty
        if (m_heldItems.Count == 0)
        {
            Debug.Log("[Inventory] Nothing to use.");
            return false;
        }

        ItemDataSO first = m_heldItems[0];
        m_heldItems.RemoveAt(0);
        _onInventoryChanged?.Invoke();
        Debug.Log($"[Inventory] Used first item: {first}");
        return true;
    }

    /// <summary>
    /// Returns true if the player holds at least one of this type.
    /// </summary>
    public bool Has(ItemDataSO type) => m_heldItems.Contains(type);

    /// <summary>
    /// Debug helper — logs all held items to the console.
    /// </summary>
    public void LogContents()
    {
        if (m_heldItems.Count == 0)
        {
            Debug.Log("[Inventory] Empty.");
            return;
        }

        // foreach loops over a List cleanly — no index needed
        foreach (ItemDataSO item in m_heldItems)
            Debug.Log($"  - {item}");
    }

    ///// public getter function to access the list of held items
    ///// this can be used by other scripts to get the contents of the inventory,
    public System.Collections.Generic.List<ItemDataSO> GetItems()
    {
        return m_heldItems;
    }
}
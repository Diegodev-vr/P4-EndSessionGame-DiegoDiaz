using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    ///// references to the item data for each item type, these will be set in the inspector
    ///// each item data will contain the information for that item type,
    ///// such as its name, icon, and value, which can be used by the inventory and
    ///// other systems that need to access item data
    [SerializeField] private ItemDataSO Orange;
    [SerializeField] private ItemDataSO Green;
    [SerializeField] private ItemDataSO Blue;
    [SerializeField] private ItemDataSO Black;
    
    ///// this dictionary will hold the item data for each item type,
    ///// using the item type as a key to look up the corresponding item data
    ///// m_items will be initialized in the Awake function by building the database from the serialized item data references
    private Dictionary<ItemType, ItemDataSO> m_items;

    ///// singleton pattern for easy access from other scripts, like the inventory script
    ///// this will allow other scripts to easily look up item data by type, without having to
    ///// reference the item database component directly,
    ///// they can just call ItemDatabase.Instance.TryGetItemData(type, out data) to get the item data for a given type
    ////// this is especially useful for the inventory script,
    ///// which needs to look up item data when picking up items and using items,
    ///// to display the item name in the logs and to determine the effects of using an item
    public static ItemDatabase Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of ItemDatabase found! Destroying the new one.");
            Destroy(gameObject);
            return;
        }
        ///// set the singleton instance to this object, so that it can be accessed from other scripts
        Instance = this;

        ///// build the item database by populating the m_items dictionary with the item data for each item type
        BuildDatabase();
    }
    
    private void BuildDatabase()
    {
        ///// initialize the m_items dictionary and populate it with the item data for each item type,
        ///// using the serialized references set in the inspector
        m_items = new Dictionary<ItemType, ItemDataSO>
        {
            ///// populate the dictionary with the item data for each item type, using the serialized references set in the inspector
            ///// the key is the item type, and the value is the corresponding item data reference
            ///// this will allow other scripts to look up the item data for a given item type by using the TryGetItemData function
            ////// for example, the inventory script can call ItemDatabase.Instance.TryGetItemData(ItemType.Orange, out data)
            ///// to get the item data for the orange item type
            { ItemType.Orange, Orange },
            { ItemType.Green, Green },
            { ItemType.Blue, Blue },
            { ItemType.Black, Black }
        };
    }

    ///// this function will be used by other scripts to look up the item data for a given item type,
    ///// it will return true if the item type is found in the database, and false if it is not found
    ///// the item data will be returned through the out parameter, which will
    ///// be set to the corresponding item data if the item type is found, or to a default value if
    public bool TryGetItemData(ItemType type, out ItemDataSO itemData)
    {
        return m_items.TryGetValue(type, out itemData);
    }

    ///// this function can be used to check if a given item type exists in the database,
    ///// it will return true if the item type is found, and false if it is not found
    public bool ContainsItemType(ItemType type)
    {
        return m_items.ContainsKey(type);
    }
}

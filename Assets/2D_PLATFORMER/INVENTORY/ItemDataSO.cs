using UnityEngine;

///// this scriptable object will hold the data for each item type, such as its name, icon, and value,
///// this will be used in the item database to store the information for each item type,
///// and can be referenced by the inventory and other systems that need to access item data.
///// create menu attribute allows us to create new item data assets from the Unity editor
/////  which can be used to populate the item database with the data for each item type
[CreateAssetMenu(fileName = "New Item Data", menuName = "Inventory/Item Data")]

public class ItemDataSO : ScriptableObject
{
    ///// the type of the item, this will be used as a key to look up the item data in the item database
    [SerializeField] private ItemType type;
    [SerializeField] private string ItemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int value;

    ///// public getter functions to access the item data fields
    ///// these can be used by other scripts to get the information for each item type
    public ItemType Type() => type;
    public string Name() => ItemName;
    public Sprite Icon() => icon;
    public int Value() => value;    
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    ///// this script will handle the UI for a single inventory slot,
    ///// displaying the item icon and name for the item in that slot,
    ///// and updating the UI when the item data is set or changed
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _itemText;

    void Awake()
    {

        if (_icon == null)
        {
            Debug.LogError("[InventorySlotUI] Icon reference is missing!");
        }
        if (_itemText == null)
        {
            Debug.LogError("[InventorySlotUI] Item text reference is missing!");
        }
    }

    ///// this function will be called to set the item data for this inventory slot,
    public void SetData(ItemDataSO itemData)
    {
        if (itemData == null)
        {
            _icon.sprite = null;
            _itemText.text = "";
            return;
        }
        else
        {
            ///// set the icon and item name text based on the item data
            ///// so that the slot displays the correct information for the item it represents
            _icon.sprite = itemData.Icon();
            _itemText.text = itemData.Name();
        }
    
    }
}

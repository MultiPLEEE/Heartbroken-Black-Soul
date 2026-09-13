using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image selectionFrame;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemAmountText;

    public InventorySlot SlotData { get; private set; }

    public void Setup(InventorySlot slot)
    {
        SlotData = slot;
        itemIcon.sprite = slot.item.sprite;
        itemNameText.text = slot.item.objectName;
        itemAmountText.text = $":{slot.amount}";
        
        SetSelected(false);
    }

    public void SetSelected(bool isSelected)
    {
        if (selectionFrame != null)
        {
            Color color = selectionFrame.color;
            color.a = isSelected ? 1f : 0f;
            selectionFrame.color = color;
        }
    }
}
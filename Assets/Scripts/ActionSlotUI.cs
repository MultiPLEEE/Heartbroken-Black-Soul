using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ActionType
{
    Attack, 
    Technique,
    Guard, 
    Items,
    Run
}

public class ActionSlotUI : MonoBehaviour
{
    [SerializeField] private Image selectionFrame;
    [SerializeField] private TMP_Text actionText;
    
    public ActionType ActionType { get; private set; }
    public bool IsDisabled { get; private set; }

    public void Setup(ActionType setupActionType)
    {
        ActionType = setupActionType;
        
        switch (setupActionType)
        {
            case ActionType.Attack:
                actionText.text = "Attack";
                break;
            case ActionType.Technique:
                actionText.text = "Technique";
                SetDisable(true);
                break;
            case ActionType.Guard:
                actionText.text = "Guard";
                break;
            case ActionType.Items:
                actionText.text = "Items";
                break;
            case ActionType.Run:
                actionText.text = "Run";
                SetDisable(true);
                break;
        }
        
        SetSelected(false);
    }
    
    public void SetDisable(bool isSelected)
    {
        if (actionText != null)
        {
            IsDisabled = isSelected;
            actionText.color = IsDisabled? Color.gray6 : Color.white;
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (actionText != null)
        {
            Color color = selectionFrame.color;
            color.a = isSelected ? 1f : 0f;
            selectionFrame.color = color;
        }
    }
}
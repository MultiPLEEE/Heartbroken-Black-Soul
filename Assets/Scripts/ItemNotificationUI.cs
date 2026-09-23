using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemNotificationUI : MonoBehaviour
{
    public static ItemNotificationUI Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI itemTitleText;
    [SerializeField] private Image itemIcon;

    private Coroutine _hideCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        panel.SetActive(false);
    }
    
    public void ShowNotification(string title, string description, int amount, Sprite icon = null, float duration = 2f)
    {
        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
        }
        
        if (itemTitleText != null) itemTitleText.text = $"{title}     x{amount}  {description}";
        if (itemIcon != null && icon != null)
        {
            itemIcon.sprite = icon;
            itemIcon.gameObject.SetActive(true);
        }

        panel.SetActive(true);
        
        _hideCoroutine = StartCoroutine(HideAfterDelay(duration));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(false);
        _hideCoroutine = null;
    }
}
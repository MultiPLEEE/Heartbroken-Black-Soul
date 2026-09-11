using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ConsumableItemSO itemToGive;
    [SerializeField] private int amount = 1;
    [SerializeField] bool isDestroys;
    // [SerializeField] private SpriteRenderer spriteRenderer;
    // [SerializeField] private Collider2D itemCollider;
    //
    // private float fadeDuration = 0.4f;
    
    public void Interact(Player player)
    {
        if (itemToGive == null)
        {
            Debug.LogWarning($"На объекте {gameObject.name} не назначен предмет!");
            return;
        }
        
        Inventory playerInventory = player.GetPlayerInventory();
        playerInventory.AddItem(itemToGive, amount);

        if (isDestroys)
        {
            Destroy(gameObject);
        }
    }
}

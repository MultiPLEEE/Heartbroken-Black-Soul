using UnityEngine;

[CreateAssetMenu()]
public class ConsumableItemSO : ScriptableObject
{
    public Sprite sprite;
    public string objectName;
    public string objectEffectDescription;
    public string objectDescription;
    public bool isTakesTurn;
    [SerializeReference, SerializeReferenceDropdown] public IEffect effect;
}

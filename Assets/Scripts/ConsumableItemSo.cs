using UnityEngine;

[CreateAssetMenu()]
public class ConsumableItemSo : ScriptableObject
{
    public Sprite sprite;
    public string objectName;
    public string objectEffectDescription;
    public string objectDescription;
    public bool isTakesTurn;
    public AudioClip soundEffect;
    [SerializeReference, SerializeReferenceDropdown] public IEffect effect;
}

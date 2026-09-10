using UnityEngine;

[CreateAssetMenu()]
public class ScriptableItemSO : ScriptableObject
{
    public Sprite sprite;
    public string objectName;
    
    public virtual void Use()
    {
        Debug.Log($"Использован базовый предмет: {objectName}");
    }
}

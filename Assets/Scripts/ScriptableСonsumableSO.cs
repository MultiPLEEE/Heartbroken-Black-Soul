using UnityEngine;

[CreateAssetMenu()]
public class ScriptableСonsumableSO : ScriptableObject
{
    public Sprite sprite;
    public string objectName;
    
    public virtual void Use()
    {
        Debug.Log($"Использован базовый предмет: {objectName}");
    }
}

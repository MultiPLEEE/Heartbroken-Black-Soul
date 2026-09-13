using UnityEngine;

[CreateAssetMenu()]
public class AllySO : ScriptableObject
{
    [Header("Visual & Info")] public string allyName;
    public Sprite portrait;
    public Sprite battleSprite;

    [Header("Base Stats")] public int maxHp;
    public int maxMp;
    public int attack;
    public int defense;
    public float speed;
}
using UnityEngine;

[CreateAssetMenu()]
public class SoundDatabaseSo : ScriptableObject
{
    public AudioClip uiConfirm;
    public AudioClip uiPrevious;
    public AudioClip uiMoveCursor;
    
    public AudioClip playerStep;

    public AudioClip itemGet;
    
    public AudioClip attack;
    public AudioClip enemyAttack;
    public AudioClip enemyGunAttack;
    public AudioClip enemyGunReload;
}
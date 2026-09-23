using System;
using UnityEngine;

[Serializable]
public class Ally
{
    public int CurrentMaxHp { get; set; }
    public int CurrentHp { get; set; }
    public int CurrentMaxMp { get; set; }
    public int CurrentMp { get; set; }
    public float ActionGauge { get; set; }
    
    public int CurrentAttack { get; set; }
    public int CurrentMagic { get; set; }
    public int CurrentDefence { get; set; }
    public float CurrentSpeed { get; set; }
    
    public bool IsGuard { get; set; }

    public bool IsReadyToAct => ActionGauge >= 100f;
    public bool IsDead => CurrentHp <= 0;

    public Ally(AllySo baseData)
    {
        
        CurrentHp = baseData.maxHp;
        CurrentMaxHp = baseData.maxHp;
        CurrentMp = baseData.maxMp;
        CurrentMaxMp = baseData.maxMp;
        ActionGauge = 0f;

        CurrentAttack = baseData.attack;
        CurrentMagic = baseData.magic;
        CurrentDefence = baseData.defense;
        CurrentSpeed = baseData.speed;
    }
    
    public void TickActionGauge(float deltaTime)
    {
        if (IsDead) return;
        
        ActionGauge += CurrentSpeed * deltaTime;
        ActionGauge = Mathf.Clamp(ActionGauge, 0f, 100f);
    }

    public void ResetGauge()
    {
        ActionGauge = 0f;
    }

    public void TakeDamage(int amount)
    {
        int finalDamage = Mathf.Max(1, amount - CurrentDefence * (IsGuard? 3 : 1));
        CurrentHp = Mathf.Clamp(CurrentHp - finalDamage, 0, CurrentMaxHp);
    }
}
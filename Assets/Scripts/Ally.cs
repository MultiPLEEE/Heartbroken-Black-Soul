using System;
using UnityEngine;

[Serializable]
public class Ally
{
    public AllySO data;
    
    public int CurrentHp { get; set; }
    public int CurrentMp { get; set; }
    public float ActionGauge { get; set; }

    public bool IsReadyToAct => ActionGauge >= 100f;
    public bool IsDead => CurrentHp <= 0;

    public Ally(AllySO baseData)
    {
        data = baseData;
        CurrentHp = baseData.maxHp;
        CurrentMp = baseData.maxMp;
        ActionGauge = 0f;
    }
    
    public void TickActionGauge(float deltaTime)
    {
        if (IsDead) return;
        
        ActionGauge += data.speed * deltaTime;
        ActionGauge = Mathf.Clamp(ActionGauge, 0f, 100f);
    }

    public void ResetGauge()
    {
        ActionGauge = 0f;
    }

    public void TakeDamage(int amount)
    {
        int finalDamage = Mathf.Max(1, amount - data.defense);
        CurrentHp = Mathf.Clamp(CurrentHp - finalDamage, 0, data.maxHp);
    }
}
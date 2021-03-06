using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public int Level { get; set; }
    [field: SerializeField] public float Hp { get; set; }
    [field: SerializeField] public float Attack { get; set; }
    [field: SerializeField] public float CriticalRate { get; set; } = 5;
    [field: SerializeField] public float CriticalDamage { get; set; } = 1.5f;

    [field: SerializeField] public bool IsDead { get; set; }
    public float GetDefaultDamage()
    {
        var isCritical = UnityEngine.Random.Range(0f, 100f) <= CriticalRate;
        return Attack * (isCritical ? CriticalDamage : 1);
    }

    public virtual void GetDamage(float damage){ }
    
    public virtual void GetKnockBack(float knockBack, float inputRotation){ }

    private Vector2 RotationToUnitVector(float forceDirection)
    {
        var x = (float) Math.Cos(forceDirection);
        var y = (float) Math.Sin(forceDirection);
        return new Vector2(x, y);
    }
    
    
    public virtual void GetStun(float stunTime){ }
}

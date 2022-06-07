using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [field:SerializeField] protected PlayerSkillController TargetObject { get; set; }
    public int SkillOverlap { get; set; }
    public float SkillDelay { get; set; }
    public float SkillActiveTime { get; set; }
    public bool CanActivate { get; set; } = true;

    public virtual void Initialize()
    {
        throw new NotImplementedException();
    }

    public virtual void SkillActive(bool isLeft)
    {
        throw new NotImplementedException();
    }
    
    public virtual void SkillInactive()
    {
        throw new NotImplementedException();
    }
}

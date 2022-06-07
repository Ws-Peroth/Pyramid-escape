using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSwordSkill : Skill
{
    private const PoolCode ObjectPoolType = PoolCode.ThrowSwordSkill;
    private const float SkillRange = 60f;
    private const float Speed = 15f;
    private const float Damage = 10f;
    private const float KnockBack = 1f;

    public override void Initialize()
    {
        SkillDelay = 0.5f;
    }

    public override void SkillActive(bool isLeft)
    {
        const float defaultRotation = -30;
        const float changeValue = 7.5f;
        
        for (var i = 0; i < 5; i++)
        {
            var skillObject = PoolManager.instance.CreatPrefab(ObjectPoolType);
            var skill = skillObject.GetComponent<ThrowObject>();
            skillObject.SetActive(true);
            
            skill.Initialize(
                TargetObject.GetSkillRange(SkillRange),
                TargetObject.GetSkillSpeed(Speed),
                TargetObject.GetSkillDamage(Damage),
                TargetObject.GetSkillKnockBack(KnockBack)
            );
            var rotationZ = defaultRotation - changeValue * i;
            var rotationValue = new Vector3(0, isLeft ? 180 : 0, rotationZ);
            skillObject.transform.rotation = Quaternion.Euler(rotationValue);
            skillObject.transform.position = TargetObject.transform.position;
        }

        var skillDelay = TargetObject.GetSkillDelayTime(SkillDelay);
        StartCoroutine(GameUIManager.instance.SkillDelay(skillDelay, SkillKnid.E));
        Invoke(nameof(SkillInactive), skillDelay);
        CanActivate = false;
    }
    public override void SkillInactive() => CanActivate = true;
}

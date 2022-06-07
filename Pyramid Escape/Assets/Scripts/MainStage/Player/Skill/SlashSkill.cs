using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSkill : Skill
{
    private const PoolCode ObjectPoolType = PoolCode.SlashSkill;
    private const float SkillRange =30f;
    private const float Speed = 9f;
    private const float Damage = 10f;
    private const float KnockBack = 3f;

    public override void Initialize()
    {
        SkillDelay = 1f;
    }

    public override void SkillActive(bool isLeft)
    {
        var skillObject = PoolManager.instance.CreatPrefab(ObjectPoolType);
        var skill = skillObject.GetComponent<SlashObject>();
        skillObject.SetActive(true);
        skill.Initialize(
            TargetObject.GetSkillRange(SkillRange),
            TargetObject.GetSkillSpeed(Speed),
            TargetObject.GetSkillDamage(Damage),
            TargetObject.GetSkillKnockBack(KnockBack),
            isLeft
        );
        skillObject.transform.position = TargetObject.transform.position;

        var skillDelay = TargetObject.GetSkillDelayTime(SkillDelay);
        StartCoroutine(GameUIManager.instance.SkillDelay(skillDelay, SkillKnid.R));
        Invoke(nameof(SkillInactive), skillDelay);
        CanActivate = false;
    }
    public override void SkillInactive() => CanActivate = true;
}

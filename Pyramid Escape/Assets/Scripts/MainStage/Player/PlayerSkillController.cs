using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    
    private const KeyCode SkillKeyCodeQ = KeyCode.Z;
    private const KeyCode SkillKeyCodeE = KeyCode.X;
    private const KeyCode SkillKeyCodeR = KeyCode.C;
    [field: SerializeField] public Skill SkillQ { get; set; }
    [field: SerializeField] public Skill SkillE { get; set; }
    [field: SerializeField] public Skill SkillR { get; set; }

    public float GetSkillRange(float defaultRange) => defaultRange;
    public float GetSkillSpeed(float defaultSpeed) => defaultSpeed;
    public float GetSkillDamage(float defaultDamage) => defaultDamage;
    public float GetSkillKnockBack(float defaultBack) => defaultBack;
    public float GetSkillDelayTime(float defaultBack) => defaultBack;
    public float GetSkillActiveTime(float defaultBack) => defaultBack;
    
    public void Start()
    {
        SkillInput();
    }
    
    private void SkillInput()
    {
        SkillQ.Initialize();
        SkillE.Initialize();
        SkillR.Initialize();
        
        var inputSkillQ =
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(SkillKeyCodeQ))
                .Where(_ => SkillQ.CanActivate)
                .Subscribe(x =>
                {
                    SkillQ.SkillActive(playerSpriteRenderer.flipX);
                });

        var inputSkillE =
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(SkillKeyCodeE))
                .Where(_ => SkillE.CanActivate)
                .Repeat()
                .Subscribe(x =>
                {
                    SkillE.SkillActive(playerSpriteRenderer.flipX);
                });

        var inputSkillR =
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(SkillKeyCodeR))
                .Where(_ => SkillR.CanActivate)
                .Repeat()
                .Subscribe(x =>
                {
                    SkillR.SkillActive(playerSpriteRenderer.flipX);
                });
    }
}

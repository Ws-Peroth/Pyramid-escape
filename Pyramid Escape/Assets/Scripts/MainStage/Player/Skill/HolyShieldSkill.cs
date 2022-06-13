using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class HolyShieldSkill : Skill
{
    [SerializeField] private List<GameObject> orbitObjects = new List<GameObject>();
    [SerializeField] private float radius;
    [SerializeField] private float speed;

    private Coroutine _skill;
    private readonly WaitForSeconds _waitSeconds = new WaitForSeconds(0.02f);

    public override void Initialize()
    {
        SkillDelay = 11;
        SkillActiveTime = 10;
    }
    
    private void ResetPosition()
    {
        transform.position = TargetObject.transform.position;
        var angle = 360f / orbitObjects.Count;
        for (var i = 0; i < orbitObjects.Count; i++)
        {
            var h = Mathf.Sin(angle * Mathf.Deg2Rad * i) * radius;
            var w = Mathf.Cos(angle * Mathf.Deg2Rad * i) * radius;
            var setPosition = new Vector3(h, 0, w) + transform.position;
            orbitObjects[i].transform.rotation = Quaternion.Euler(new Vector3(0, angle * i, 0));
            orbitObjects[i].transform.position = setPosition;   
        }
    }

    private void Update()
    {
        transform.position = TargetObject.transform.position;
    }

    private IEnumerator OrbitLoop()
    {
        while (true)
        {
            transform.Rotate(0, speed, 0);
            yield return null;
        }
    }
    
    public override void SkillActive(bool isLeft)
    {
        GameManager.instance.IsShield = true;
        SkillOverlap++;
        foreach (var t in orbitObjects)
        {
            t.SetActive(true);
        }
        ResetPosition();
        CanActivate = false;
        _skill = StartCoroutine(OrbitLoop());
        var skillDelay = TargetObject.GetSkillDelayTime(SkillDelay);
        Invoke(nameof(SkillEnd), skillDelay);   // 쿨타임이 끝났을 때
        StartCoroutine(GameUIManager.instance.SkillDelay(skillDelay, SkillKnid.Q));
        Invoke(nameof(SkillInactive), TargetObject.GetSkillActiveTime(SkillActiveTime)); // 보호막 시간이 끝났을 때
    }

    private void SkillEnd()
    {
        CanActivate = true;
    }
    
    public override void SkillInactive()
    {
        GameManager.instance.IsShield = false;
        SkillOverlap--;
        if (SkillOverlap > 0)
        {
            return;
        }
        foreach (var orbitObject in orbitObjects)
        {
            orbitObject.SetActive(false);
        }
        StopCoroutine(_skill);
    }
}

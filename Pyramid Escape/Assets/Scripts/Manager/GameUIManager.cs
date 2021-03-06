using System;
using System.Collections;
using System.Collections.Generic;
using MainStage.MapMaker;
using UnityEngine;
using UnityEngine.UI;

public enum SkillKnid
{
    Q = 0,
    E = 1,
    R = 2
}

public class GameUIManager : Singleton<GameUIManager>
{
    private const float HeartHp = 10;
    private readonly bool[] _isSkillDelayUIOn = new bool[3] {false, false, false};
    
    [SerializeField] public GameObject panel;
    [SerializeField] private Image[] skillFillImages = new Image[3];
    [SerializeField] private Image[] hpUI = new Image[5];
    [SerializeField] private Text overHpText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text hpText;

    [SerializeField] private float time;
    
    public IEnumerator SkillDelay(float skillDelay,  SkillKnid kind)
    {
        var index = (int) kind;
        if (_isSkillDelayUIOn[index])
        {
            yield break;
        }
        _isSkillDelayUIOn[index] = true;
        
        var icon = skillFillImages[index];
        var currentSkillDelay = skillDelay;
        while (currentSkillDelay > 0)
        {
            currentSkillDelay -= Time.deltaTime;
            icon.fillAmount = currentSkillDelay / skillDelay;
            yield return null;
        }
        _isSkillDelayUIOn[index] = false;
    }
    private void HeartActive(int count, float remain)
    {
        for (var i = 0; i < 5; i++)
        {
            var fill = i < count ? 1 : 0f;
            fill = i == count ? remain / HeartHp : fill;
            hpUI[i].fillAmount = fill;
        }
    }

    public void UpdateHpUI(float hp)
    {
        hpText.text = $"Hp : {((int) hp).ToString()}";
        var heartCount = (int) (hp / HeartHp);
        var remainHp = hp % HeartHp;
        HeartActive(heartCount, remainHp);
        if (heartCount <= 5)
        {
            overHpText.text = "";
            return;
        }
        var v = (int) (heartCount + Mathf.Ceil(remainHp));
        overHpText.text = $"+{v.ToString()}";
    }

    public void SetGoldText(int gold)
    {
        goldText.text = gold.ToString();
    }
    
    private void Start()
    {
        timeText.text = "00 : 00 : 00";
        goldText.text = "0";
        time = 0;
        SetGoldText(0);
    }

    private void Update()
    {
        if (MapDataInitializer.instance.GenerateFinish)
        {
            time += Time.deltaTime;
            timeText.text =
                $"{time / 3600:00} : {time / 60 % 60:00} : {time % 60:00}";
        }
    }
}

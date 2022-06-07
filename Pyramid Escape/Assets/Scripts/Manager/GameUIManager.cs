using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Image[] skillFillImages = new Image[3];
    private readonly bool[] _isSkillDelayUIOn = new bool[3] {false, false, false};

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
    
}

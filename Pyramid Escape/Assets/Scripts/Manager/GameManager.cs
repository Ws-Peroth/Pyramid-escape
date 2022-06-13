using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public IDisposable inputSkillQ;
    public  IDisposable inputSkillE;
    public  IDisposable inputSkillR;
    
    public int Gold { get; set; }
    public bool IsShield { get; set; }

    private void Start()
    {
        GetGold(0);
    }

    public void GetGold(int gold)
    {
        Gold += gold;
        GameDataManager.instance.Gold = Gold;
        GameUIManager.instance.SetGoldText(Gold);
    }

    public void GameEnd()
    {
        inputSkillQ.Dispose();
        inputSkillE.Dispose();
        inputSkillR.Dispose();
        GameDataManager.instance.Gold = Gold;
        GameUIManager.instance.panel.SetActive(true);
        SceneManager.LoadScene((int) Scenes.End);
    }
}

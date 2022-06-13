using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alter : ActiveObject
{
    private bool isAc = false;
    
    public override void Activate(GameObject player)
    {
        if(isAc) return;
        isAc = true;
        player.GetComponent<Player>().Attack += 10;
        player.GetComponent<Player>().Hp += 10;
        GameUIManager.instance.UpdateHpUI(player.GetComponent<Player>().Hp);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : ActiveObject
{
    private bool isAc = false;
    public override void Activate(GameObject player)
    {
        if(isAc) return;
        isAc = true;
        GameManager.instance.GameEnd();
    }
}

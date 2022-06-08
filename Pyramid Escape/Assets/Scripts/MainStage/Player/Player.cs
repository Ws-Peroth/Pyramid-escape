using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public const int PlayerSortOrder = 0;
    void Start()
    {
        Hp = 50;
        GameUIManager.instance.UpdateHpUI(Hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        Hp -= damage;
        GameUIManager.instance.UpdateHpUI(Hp);
    }
}
